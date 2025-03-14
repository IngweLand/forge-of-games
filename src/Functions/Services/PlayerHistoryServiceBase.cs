using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services;

public interface IPlayerHistoryServiceBase
{
    Task UpdateAsync(IEnumerable<PlayerAggregate> playerAggregates);
}

public abstract class PlayerHistoryServiceBase(IFogDbContext context, ILogger<PlayerRankingService> logger) : IPlayerHistoryServiceBase
{
    protected abstract Func<IQueryable<Player>, IQueryable<Player>> IncludeQuery { get; }

    public async Task UpdateAsync(IEnumerable<PlayerAggregate> playerAggregates)
    {
        logger.LogInformation("UpdateAsync started processing aggregates");
        var filtered = Filter(playerAggregates);
        if (filtered.Count == 0)
        {
            logger.LogInformation("No aggregates to process after filtering");
            return;
        }
        var grouped = filtered.GroupBy(p => p.Key);

        foreach (var chunk in grouped.Chunk(1000))
        {
            logger.LogInformation("Processing chunk with {ChunkSize} items", chunk.Length);
            var inGamePlayerIds = chunk.Select(g => g.Key.InGamePlayerId).ToHashSet();
            logger.LogDebug("Retrieving existing players for {IdCount} in-game player IDs", inGamePlayerIds.Count);
            var existingPlayers = await FindExistingPlayers(inGamePlayerIds);
            logger.LogDebug("Retrieved {ExistingCount} existing players", existingPlayers.Count);
            foreach (var playerGroup in chunk)
            {
                if (existingPlayers.TryGetValue(playerGroup.Key, out var existingPlayer))
                {
                    ProcessExistingPlayer(existingPlayer, playerGroup);
                    logger.LogDebug("Processed existing player with key {PlayerKey}", playerGroup.Key);
                }
                else
                {
                    logger.LogWarning("No existing player found for key {PlayerKey}", playerGroup.Key);
                }
            }
            logger.LogInformation("Saving changes for the current chunk.");
            await context.SaveChangesAsync();
            logger.LogInformation("Successfully saved changes for the current chunk.");
        }
        logger.LogInformation("UpdateAsync completed processing aggregates");
    }

    protected abstract List<PlayerAggregate> Filter(IEnumerable<PlayerAggregate> playerAggregates);
    protected abstract void ProcessExistingPlayer(Player existingPlayer, IEnumerable<PlayerAggregate> playerAggregates);

    private async Task<Dictionary<PlayerKey, Player>> FindExistingPlayers(HashSet<int> inGamePlayerIds)
    {
        var query = context.Players.AsQueryable();
        var players = await IncludeQuery(query)
            .Where(p => inGamePlayerIds.Contains(p.InGamePlayerId))
            .ToListAsync();
        return players.ToDictionary(p => p.Key);
    }
}
