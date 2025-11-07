using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Alliance = Ingweland.Fog.Models.Fog.Entities.Alliance;

namespace Ingweland.Fog.Functions.Services;

public interface IAllianceRankingService
{
    Task AddOrUpdateRankingsAsync(IEnumerable<AllianceAggregate> allianceAggregates);
}

public class AllianceRankingService(IFogDbContext context, IMapper mapper, ILogger<AllianceRankingService> logger)
    : IAllianceRankingService
{
    public async Task AddOrUpdateRankingsAsync(IEnumerable<AllianceAggregate> allianceAggregates)
    {
        var filtered = allianceAggregates.Where(a => a.CanBeConvertedToAllianceRanking()).ToList();
        logger.LogInformation("Filtered to {FilteredCount} valid alliance aggregates.", filtered.Count);
        if (filtered.Count == 0)
        {
            logger.LogInformation("No valid alliance aggregates to process. Exiting method.");
            return;
        }

        var latestUnique = filtered
            .GroupBy(p => (p.WorldId, p.InGameAllianceId, p.AllianceRankingType, DateOnly.FromDateTime(p.CollectedAt)))
            .Select(g => g.OrderByDescending(p => p.CollectedAt).First())
            .ToList();
        logger.LogInformation("Latest unique aggregates count: {Count}.", latestUnique.Count);

        int updatedAllianceCount = 0, updatedRankingCount = 0, addedRankingCount = 0;

        foreach (var chunk in latestUnique.Chunk(500))
        {
            logger.LogInformation("Processing a chunk of {ChunkCount} aggregates.", chunk.Count());
            var inGameAllianceIds = chunk.Select(p => p.InGameAllianceId).ToHashSet();
            var inGameLeaderIds = chunk.Where(a => a.LeaderInGameId.HasValue).Select(p => p.LeaderInGameId!.Value)
                .ToHashSet();
            var earliestDate = chunk.OrderBy(p => p.CollectedAt).Select(p => p.CollectedAt).First();

            var existingAlliances = await FindExistingAlliances(inGameAllianceIds, earliestDate);
            logger.LogInformation("Found {Count} existing alliances for current chunk.", existingAlliances.Count);
            var leaderIds = await FindLeaders(inGameLeaderIds);
            logger.LogInformation("Found {Count} leader IDs.", leaderIds.Count);

            foreach (var allianceAggregate in chunk)
            {
                if (existingAlliances.TryGetValue(allianceAggregate.Key, out var existingAlliance))
                {
                    var date = DateOnly.FromDateTime(allianceAggregate.CollectedAt);
                    var existingRanking =
                        existingAlliance.Rankings.FirstOrDefault(r =>
                            r.CollectedAt == date && r.Type == allianceAggregate.AllianceRankingType);
                    if (existingRanking != null)
                    {
                        existingRanking.Points = allianceAggregate.RankingPoints!.Value;
                        existingRanking.Rank = allianceAggregate.Rank!.Value;
                        updatedRankingCount++;
                    }
                    else
                    {
                        existingAlliance.Rankings.Add(mapper.Map<AllianceRanking>(allianceAggregate));
                        addedRankingCount++;
                    }

                    if (date >= existingAlliance.UpdatedAt)
                    {
                        if (allianceAggregate.AllianceRankingType == AllianceRankingType.MemberTotal)
                        {
                            existingAlliance.RankingPoints = allianceAggregate.RankingPoints!.Value;
                            existingAlliance.Rank = allianceAggregate.Rank!.Value;
                            existingAlliance.UpdatedAt = date;
                        }

                        if (allianceAggregate.RegisteredAt.HasValue)
                        {
                            existingAlliance.RegisteredAt = allianceAggregate.RegisteredAt!.Value;
                        }

                        updatedAllianceCount++;
                    }
                }
                else
                {
                    logger.LogWarning("Alliance with key {AllianceKey} not found.", allianceAggregate.Key);
                }
            }

            await context.SaveChangesAsync();
        }

        logger.LogInformation(
            "Finished processing alliance aggregates. Updated Alliances: {UpdatedAllianceCount}, Updated Rankings: {UpdatedRankingCount}, Added Rankings: {AddedRankingCount}.",
            updatedAllianceCount, updatedRankingCount, addedRankingCount);
    }

    private async Task<Dictionary<AllianceKey, Alliance>> FindExistingAlliances(HashSet<int> inGameAllianceIds,
        DateTime earliestDate)
    {
        var queryDate = DateOnly.FromDateTime(earliestDate).AddDays(-1);
        logger.LogInformation(
            "Querying existing alliances for {Count} in-game alliance IDs with collected date after {QueryDate}.",
            inGameAllianceIds.Count, queryDate);
        var alliances = await context.Alliances
            .Include(p => p.Rankings.Where(pr => pr.CollectedAt > queryDate))
            .Where(p => inGameAllianceIds.Contains(p.InGameAllianceId))
            .ToListAsync();
        logger.LogInformation("Found {AllianceCount} alliances from DB.", alliances.Count);
        return alliances.ToDictionary(p => p.Key);
    }

    private async Task<Dictionary<PlayerKey, Player>> FindLeaders(HashSet<int> inGamePlayerIds)
    {
        logger.LogInformation("Querying leader IDs for {Count} in-game player IDs.", inGamePlayerIds.Count);
        var players = await context.Players
            .Where(p => inGamePlayerIds.Contains(p.InGamePlayerId))
            .ToListAsync();
        logger.LogInformation("Found {PlayerCount} players.", players.Count);
        return players.ToDictionary(p => new PlayerKey(p.WorldId, p.InGamePlayerId));
    }
}
