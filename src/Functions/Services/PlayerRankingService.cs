using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Player = Ingweland.Fog.Models.Fog.Entities.Player;

namespace Ingweland.Fog.Functions.Services;

public interface IPlayerRankingService
{
    Task AddOrUpdateRankingsAsync(IEnumerable<PlayerAggregate> playerAggregates);
}

public class PlayerRankingService(IFogDbContext context, IMapper mapper, ILogger<PlayerRankingService> logger)
    : IPlayerRankingService
{
    public async Task AddOrUpdateRankingsAsync(IEnumerable<PlayerAggregate> playerAggregates)
    {
        var filtered = playerAggregates.Where(p => p.CanBeConvertedToPlayerRanking()).ToList();
        if (filtered.Count == 0)
        {
            logger.LogInformation("No valid player aggregates to process.");
            return;
        }

        var latestUnique = filtered
            .GroupBy(p => (p.WorldId, p.InGamePlayerId, p.PlayerRankingType, DateOnly.FromDateTime(p.CollectedAt)))
            .Select(g => g.OrderByDescending(p => p.CollectedAt).First())
            .ToList();

        int updatedPlayerCount = 0, updatedRankingCount = 0, addedRankingCount = 0;

        foreach (var chunk in latestUnique.Chunk(500))
        {
            logger.LogInformation("Processing chunk with {ChunkSize} items", chunk.Length);
            var inGamePlayerIds = chunk.Select(p => p.InGamePlayerId).ToHashSet();
            var earliestDate = chunk.OrderBy(p => p.CollectedAt).Select(p => p.CollectedAt).First();

            var existingPlayers = await FindExistingPlayers(inGamePlayerIds, earliestDate);
            logger.LogInformation("Found {ExistingPlayerCount} existing players for the current chunk",
                existingPlayers.Count);

            foreach (var playerAggregate in chunk)
            {
                if (existingPlayers.TryGetValue(playerAggregate.Key, out var existingPlayer))
                {
                    var date = DateOnly.FromDateTime(playerAggregate.CollectedAt);
                    var existingRanking =
                        existingPlayer.Rankings.FirstOrDefault(r =>
                            r.CollectedAt == date && r.Type == playerAggregate.PlayerRankingType);
                    if (existingRanking != null)
                    {
                        existingRanking.Points = playerAggregate.RankingPoints!.Value;
                        existingRanking.Rank = playerAggregate.Rank!.Value;
                        updatedRankingCount++;
                    }
                    else
                    {
                        existingPlayer.Rankings.Add(mapper.Map<PlayerRanking>(playerAggregate));
                        addedRankingCount++;
                    }

                    if (playerAggregate.PlayerRankingType == PlayerRankingType.TotalHeroPower &&
                        date >= existingPlayer.UpdatedAt)
                    {
                        existingPlayer.RankingPoints = playerAggregate.RankingPoints!.Value;
                        existingPlayer.Rank = playerAggregate.Rank!.Value;
                        existingPlayer.UpdatedAt = date;
                        existingPlayer.Status = InGameEntityStatus.Active;
                        updatedPlayerCount++;
                    }
                }
                else
                {
                    logger.LogWarning("Player with key {PlayerKey} not found in existing players", playerAggregate.Key);
                }
            }
            
            await context.SaveChangesAsync();
        }

        logger.LogInformation(
            "Completed processing: {UpdatedPlayers} players updated, {UpdatedRankings} rankings updated, {AddedRankings} rankings added",
            updatedPlayerCount, updatedRankingCount, addedRankingCount);
    }

    private async Task<Dictionary<PlayerKey, Player>> FindExistingPlayers(HashSet<int> inGamePlayerIds,
        DateTime earliestDate)
    {
        var date = DateOnly.FromDateTime(earliestDate).AddDays(-1);
        var players = await context.Players
            .Include(p => p.Rankings.Where(pr => pr.CollectedAt > date))
            .Where(p => inGamePlayerIds.Contains(p.InGamePlayerId))
            .ToListAsync();
        logger.LogInformation("Retrieved {PlayerCount} players from database", players.Count);
        return players.ToDictionary(p => p.Key);
    }
}