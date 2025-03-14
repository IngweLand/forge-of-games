using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Player = Ingweland.Fog.Models.Fog.Entities.Player;

namespace Ingweland.Fog.Functions.Services;

public interface IPvpRankingService
{
    Task AddOrUpdateRankingsAsync(IEnumerable<PlayerAggregate> playerAggregates);
}

public class PvpRankingService(IFogDbContext context, IMapper mapper, ILogger<PvpRankingService> logger)
    : IPvpRankingService
{
    private const int TimeBucketHours = 12;

    public async Task AddOrUpdateRankingsAsync(IEnumerable<PlayerAggregate> playerAggregates)
    {
        var playerAggregatesList = playerAggregates.ToList();
        logger.LogInformation("AddOrUpdateRankingsAsync started with {Count} aggregates", playerAggregatesList.Count);
        var filtered = playerAggregatesList.Where(p => p.CanBeConvertedToPvpRanking()).ToList();
        logger.LogInformation("Filtered aggregates count: {Count}", filtered.Count);
        if (filtered.Count == 0)
        {
            logger.LogInformation("No valid player aggregates to process");
            return;
        }

        var latestUnique = filtered
            .GroupBy(p => (p.WorldId, p.InGamePlayerId, p.PlayerRankingType, AdjustCollectedTime(p.CollectedAt)))
            .Select(g => g.OrderByDescending(p => p.CollectedAt).First())
            .ToList();

        int updatedRankingCount = 0, addedRankingCount = 0;

        logger.LogInformation("Processing {Count} unique player aggregates", latestUnique.Count);

        foreach (var chunk in latestUnique.Chunk(1000))
        {
            logger.LogInformation("Processing chunk with {ChunkSize} aggregates", chunk.Length);
            var inGamePlayerIds = chunk.Select(p => p.InGamePlayerId).ToHashSet();
            var earliestDate = chunk.OrderBy(p => p.CollectedAt).Select(p => p.CollectedAt).First();
            logger.LogInformation("Earliest collected date in chunk: {EarliestDate}", earliestDate);
            var existingPlayers = await FindExistingPlayers(inGamePlayerIds, earliestDate);
            logger.LogInformation("Found {Count} existing players in current chunk", existingPlayers.Count);

            foreach (var playerAggregate in chunk)
            {
                if (existingPlayers.TryGetValue(playerAggregate.Key, out var existingPlayer))
                {
                    var date = AdjustCollectedTime(playerAggregate.CollectedAt);
                    var existingRanking = existingPlayer.PvpRankings.FirstOrDefault(r => r.CollectedAt == date);
                    if (existingRanking != null)
                    {
                        existingRanking.Points = playerAggregate.PvpRankingPoints!.Value;
                        existingRanking.Rank = playerAggregate.PvpRank!.Value;
                        updatedRankingCount++;
                        logger.LogDebug("Updated ranking for player {PlayerKey} at {Date}", playerAggregate.Key, date);
                    }
                    else
                    {
                        existingPlayer.PvpRankings.Add(new PvpRanking()
                        {
                            Rank = playerAggregate.PvpRank!.Value,
                            Points = playerAggregate.PvpRankingPoints!.Value,
                            CollectedAt = date,
                        });
                        addedRankingCount++;
                        logger.LogDebug("Added ranking for player {PlayerKey} at {Date}", playerAggregate.Key, date);
                    }
                }
                else
                {
                    logger.LogWarning("Player with key {PlayerKey} not found in existing players", playerAggregate.Key);
                }

                await context.SaveChangesAsync();
                logger.LogDebug("Saved changes for player {PlayerKey}", playerAggregate.Key);
            }
        }
        logger.LogInformation("Completed processing. Updated ranking count: {UpdatedRankingCount}, Added ranking count: {AddedRankingCount}", updatedRankingCount, addedRankingCount);
    }

    private static DateTime AdjustCollectedTime(DateTime src)
    {
        var hour = src.Hour / TimeBucketHours * TimeBucketHours;
        return new DateTime(src.Year, src.Month, src.Day, hour, 0, 0);
    }

    private async Task<Dictionary<PlayerKey, Player>> FindExistingPlayers(HashSet<int> inGamePlayerIds,
        DateTime earliestDate)
    {
        var date = earliestDate.AddDays(-1);
        logger.LogInformation("Fetching players with InGamePlayerIds count: {Count} with date filter > {Date}", inGamePlayerIds.Count, date);
        var players = await context.Players
            .Include(p => p.PvpRankings.Where(pr => pr.CollectedAt > date))
            .Where(p => inGamePlayerIds.Contains(p.InGamePlayerId))
            .ToListAsync();
        logger.LogInformation("Fetched {Count} players from database", players.Count);
        return players.ToDictionary(p => p.Key);
    }
}
