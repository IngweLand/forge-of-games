using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Models.Fog.Entities;
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
        if (filtered.Count == 0)
        {
            return;
        }

        var latestUnique = filtered
            .GroupBy(p => (p.WorldId, p.InGameAllianceId, DateOnly.FromDateTime(p.CollectedAt)))
            .Select(g => g.OrderByDescending(p => p.CollectedAt).First())
            .ToList();

        int updatedAllianceCount = 0, updatedRankingCount = 0, addedRankingCount = 0;

        foreach (var chunk in latestUnique.Chunk(1000))
        {
            var inGameAllianceIds = chunk.Select(p => p.InGameAllianceId).ToHashSet();
            var inGameLeaderIds = chunk.Where(a => a.LeaderInGameId.HasValue).Select(p => p.LeaderInGameId!.Value).ToHashSet();
            var earliestDate = chunk.OrderBy(p => p.CollectedAt).Select(p => p.CollectedAt).First();

            var existingAlliances = await FindExistingAlliances(inGameAllianceIds, earliestDate);
            var leaderIds = await FindLeaderIds(inGameLeaderIds);

            foreach (var allianceAggregate in chunk)
            {
                if (existingAlliances.TryGetValue(allianceAggregate.Key, out var existingAlliance))
                {
                    var date = DateOnly.FromDateTime(allianceAggregate.CollectedAt);
                    var existingRanking =
                        existingAlliance.Rankings.FirstOrDefault(r => r.CollectedAt == date);
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
                        existingAlliance.RankingPoints = allianceAggregate.RankingPoints!.Value;
                        existingAlliance.Rank = allianceAggregate.Rank!.Value;
                        existingAlliance.UpdatedAt = date;
                        if (allianceAggregate.RegisteredAt.HasValue)
                        {
                            existingAlliance.RegisteredAt = allianceAggregate.RegisteredAt!.Value;
                        }

                        if (allianceAggregate.LeaderInGameId.HasValue && leaderIds.TryGetValue(
                                new PlayerKey(allianceAggregate.WorldId, allianceAggregate.LeaderInGameId.Value),
                                out var leaderId))
                        {
                            existingAlliance.LeaderId = leaderId;
                        }

                        updatedAllianceCount++;
                    }
                }
                else
                {
                    // log warning
                }

                await context.SaveChangesAsync();
            }
        }
    }

    private async Task<Dictionary<AllianceKey, Alliance>> FindExistingAlliances(HashSet<int> inGameAllianceIds,
        DateTime earliestDate)
    {
        var date = DateOnly.FromDateTime(earliestDate).AddDays(-1);
        var alliances = await context.Alliances
            .Include(p => p.Rankings.Where(pr => pr.CollectedAt > date))
            .Where(p => inGameAllianceIds.Contains(p.InGameAllianceId))
            .ToListAsync();
        return alliances.ToDictionary(p => p.Key);
    }

    private async Task<Dictionary<PlayerKey, int>> FindLeaderIds(HashSet<int> inGamePlayerIds)
    {
        var alliances = await context.Players
            .Where(p => inGamePlayerIds.Contains(p.InGamePlayerId))
            .Select(p => new {p.Id, p.InGamePlayerId, p.WorldId})
            .ToListAsync();
        return alliances.ToDictionary(p => new PlayerKey(p.WorldId, p.InGamePlayerId), p => p.Id);
    }
}