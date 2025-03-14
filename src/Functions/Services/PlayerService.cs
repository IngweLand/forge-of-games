using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Player = Ingweland.Fog.Models.Fog.Entities.Player;

namespace Ingweland.Fog.Functions.Services;

public interface IPlayerService
{
    Task AddAsync(IEnumerable<PlayerAggregate> playerAggregates);
}

public class PlayerService(IFogDbContext context, IMapper mapper, ILogger<PlayerRankingService> logger) : IPlayerService
{
    public async Task AddAsync(IEnumerable<PlayerAggregate> playerAggregates)
    {
        var unique = playerAggregates
            .Where(p => p.CanBeConvertedToPlayer())
            .OrderByDescending(p => p.CollectedAt) // we need this to correctly set UpdateAt on the player
            .DistinctBy(p => p.Key)
            .ToDictionary(p => p.Key);
        logger.LogInformation("Filtered aggregates to {UniqueCount} unique items.", unique.Count);
        var existingPlayerKeys = await GetExistingPlayersAsync(unique.Keys.Select(pk => pk.InGamePlayerId).ToHashSet());
        logger.LogInformation("Found {ExistingCount} existing players.", existingPlayerKeys.Count);
        var newPlayerKeys = unique.Keys.ToHashSet().Except(existingPlayerKeys).ToList();
        logger.LogInformation("Identified {NewCount} new players.", newPlayerKeys.Count);
        var newPlayersList = newPlayerKeys.Select(k =>
        {
            var playerAggregate = unique[k];
            return new Player()
            {
                WorldId = playerAggregate.WorldId,
                InGamePlayerId = playerAggregate.InGamePlayerId,
                Name = playerAggregate.Name!,
                Age = playerAggregate.Age!,
                AvatarId = playerAggregate.AvatarId ?? 0,
                UpdatedAt = DateOnly.FromDateTime(playerAggregate.CollectedAt),
            };
        }).ToList();
        context.Players.AddRange(newPlayersList);
        await context.SaveChangesAsync();
        logger.LogInformation("Saved {NewPlayersCount} new players.", newPlayersList.Count);
    }

    private async Task<HashSet<PlayerKey>> GetExistingPlayersAsync(HashSet<int> inGamePlayerIds)
    {
        logger.LogInformation("Querying existing players for {IdCount} in-game player IDs.", inGamePlayerIds.Count);
        var existing = await context.Players
            .Where(p => inGamePlayerIds.Contains(p.InGamePlayerId))
            .ProjectTo<PlayerKey>(mapper.ConfigurationProvider)
            .ToHashSetAsync();
        logger.LogInformation("Query returned {ExistingCount} existing players.", existing.Count);
        return existing;
    }
}
