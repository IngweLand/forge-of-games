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
        var filtered = Filter(playerAggregates);

        if (filtered.Count == 0)
        {
            return;
        }

        var grouped = filtered.GroupBy(p => p.Key);

        foreach (var chunk in grouped.Chunk(1000))
        {
            var inGamePlayerIds = chunk.Select(g => g.Key.InGamePlayerId).ToHashSet();
            var existingPlayers = await FindExistingPlayers(inGamePlayerIds);

            foreach (var playerGroup in chunk)
            {
                if (existingPlayers.TryGetValue(playerGroup.Key, out var existingPlayer))
                {
                    ProcessExistingPlayer(existingPlayer, playerGroup);
                }
                else
                {
                    // log warning 
                }
            }

            await context.SaveChangesAsync();
        }
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