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
        var existingPlayerKeys = await GetExistingPlayersAsync(unique.Keys.Select(pk => pk.InGamePlayerId).ToHashSet());
        var newPlayerKeys = unique.Keys.ToHashSet().Except(existingPlayerKeys);
        var newPlayers = newPlayerKeys.Select(k =>
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
        });
        context.Players.AddRange(newPlayers);
        await context.SaveChangesAsync();
    }

    private async Task<HashSet<PlayerKey>> GetExistingPlayersAsync(HashSet<int> inGamePlayerIds)
    {
        return await context.Players
            .Where(p => inGamePlayerIds.Contains(p.InGamePlayerId))
            .ProjectTo<PlayerKey>(mapper.ConfigurationProvider)
            .ToHashSetAsync();
    }
}