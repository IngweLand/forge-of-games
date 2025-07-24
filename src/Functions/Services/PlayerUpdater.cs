using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services;

public interface IPlayerUpdater
{
    Task UpdateAsync(IEnumerable<PlayerAggregate> playerAggregates);
}

public class PlayerUpdater(IFogDbContext context, ILogger<PlayerRankingService> logger) : IPlayerUpdater
{
    public async Task UpdateAsync(IEnumerable<PlayerAggregate> playerAggregates)
    {
        var unique = playerAggregates.Where(p => p.HasRequiredPropertiesSet()).DistinctBy(x => x.Key).ToList();
        if (unique.Count == 0)
        {
            logger.LogInformation("No valid player aggregates to process.");
            return;
        }

        var inGamePlayerIds = unique.Select(p => p.InGamePlayerId).ToHashSet();

        var existingPlayers = await GetExistingPlayersAsync(inGamePlayerIds);

        foreach (var playerAggregate in unique)
        {
            if (existingPlayers.TryGetValue(playerAggregate.Key, out var existingPlayer))
            {
                existingPlayer.TreasureHuntDifficulty = playerAggregate.TreasureHuntDifficulty;
                existingPlayer.PvpTier = playerAggregate.PvpTier;
            }
            else
            {
                logger.LogWarning("Player with key {PlayerKey} not found in existing players", playerAggregate.Key);
            }
        }

        await context.SaveChangesAsync();
    }

    private async Task<Dictionary<PlayerKey, Player>> GetExistingPlayersAsync(HashSet<int> inGamePlayerIds)
    {
        logger.LogDebug("Querying existing players for {IdCount} in-game player IDs.", inGamePlayerIds.Count);
        var existing = await context.Players
            .Where(p => inGamePlayerIds.Contains(p.InGamePlayerId))
            .ToListAsync();
        logger.LogDebug("Query returned {ExistingCount} existing players.", existing.Count);
        return existing.ToDictionary(x => x.Key);
    }
}
