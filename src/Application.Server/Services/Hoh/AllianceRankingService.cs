using AutoMapper;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class AllianceRankingService(IFogDbContext context, IMapper mapper, ILogger<AllianceRankingService> logger)
    : IAllianceRankingService
{
    public async Task AddOrUpdateRangeAsync(IEnumerable<AllianceRank> rankings, string worldId, DateOnly collectedAt,
        AllianceRankingType allianceRankingType)
    {
        var rankingList = rankings as IList<AllianceRank> ?? rankings.ToList();
        if (rankingList.Count == 0)
        {
            logger.LogInformation("AddRange called with no alliance rankings to add.");
            return;
        }

        rankingList = Prepare(rankingList);

        logger.LogInformation("Starting to add/update {AllianceCount} alliance rankings.", rankingList.Count);

        var existingAlliances = await GetAlliances(rankingList, collectedAt);

        int updatedAllianceCount = 0, addedAllianceCount = 0, updatedRankingCount = 0, addedRankingCount = 0;

        foreach (var source in rankingList)
        {
            var (alliance, allianceRanking) = MapEntities(source, worldId, collectedAt, allianceRankingType);

            UpdateAllianceRankingPointsIfRequired(alliance, allianceRankingType);

            if (existingAlliances.TryGetValue(new AllianceKey(allianceRanking.WorldId, allianceRanking.InGameAllianceId),
                    out var existingAlliance))
            {
               
                if (alliance.UpdatedAt >= existingAlliance.UpdatedAt)
                {
                    mapper.Map(alliance, existingAlliance);
                    updatedAllianceCount++;
                }
               
                if (existingAlliance.Rankings.Count == 0)
                {
                    existingAlliance.Rankings.Add(allianceRanking);
                    addedRankingCount++;
                }
                else
                {
                    var existingRanking = existingAlliance.Rankings.FirstOrDefault(r => r.Key == allianceRanking.Key);
                    if (existingRanking != null)
                    {
                        mapper.Map(allianceRanking, existingRanking);
                        updatedRankingCount++;
                    }
                    else
                    {
                        existingAlliance.Rankings.Add(allianceRanking);
                        addedRankingCount++;
                    }
                }
            }
            else
            {
                alliance.Rankings.Add(allianceRanking);
                context.Alliances.Add(alliance);
                addedAllianceCount++;
                addedRankingCount++;
            }
        }

        logger.LogInformation(
            "Summary of modifications {@Summary}",
            new
            {
                addedAllianceCount, updatedAllianceCount, addedRankingCount, updatedRankingCount
            });

        await context.SaveChangesAsync();

        logger.LogInformation("Successfully saved changes to database.");
    }

    private async Task<Dictionary<AllianceKey, Alliance>> GetAlliances(IList<AllianceRank> rankingList, DateOnly date)
    {
        var inGameAllianceIds = rankingList.Select(p => p.Id).ToHashSet();
        var alliances = await context.Alliances
            .Include(p => p.Rankings.Where(pr => pr.CollectedAt == date))
            .Where(p => inGameAllianceIds.Contains(p.InGameAllianceId))
            .ToListAsync();
        return alliances.ToDictionary(p => p.Key);
    }

    private (Alliance Alliance, AllianceRanking AllianceRanking) MapEntities(AllianceRank source, string worldId,
        DateOnly collectedAt, AllianceRankingType allianceRankingType)
    {
        var allianceRanking = mapper.Map<AllianceRanking>(source, opt =>
        {
            opt.Items[ResolutionContextKeys.ALLIANCE_RANKING_TYPE] = allianceRankingType;
            opt.Items[ResolutionContextKeys.WORLD_ID] = worldId;
            opt.Items[ResolutionContextKeys.DATE] = collectedAt;
        });
        var alliance = mapper.Map<Alliance>(source, opt =>
        {
            opt.Items[ResolutionContextKeys.WORLD_ID] = worldId;
            opt.Items[ResolutionContextKeys.DATE] = collectedAt;
        });
        return (alliance, allianceRanking);
    }

    private static List<AllianceRank> Prepare(IEnumerable<AllianceRank> rankings)
    {
        return rankings
            .GroupBy(p => p.Id)
            .Select(g => g.MaxBy(p => p.Points)!)
            .ToList();
    }

    private void UpdateAllianceRankingPointsIfRequired(Alliance alliance, AllianceRankingType allianceRankingType)
    {
        switch (allianceRankingType)
        {
            case AllianceRankingType.RankingPoints:
            {
                break;
            }
            case AllianceRankingType.TotalPoints:
            {
                alliance.RankingPoints =
                    (int) Math.Round(
                        alliance.RankingPoints * HohConstants.PLAYER_TOTAL_TO_ALLIANCE_RANKING_POINTS_FACTOR,
                        MidpointRounding.AwayFromZero);
                break;
            }
            default:
            {
                logger.LogError("Unsupported alliance ranking type {allianceRankingType}", allianceRankingType);
                throw new ArgumentException($"Unsupported alliance ranking type {allianceRankingType}");
            }
        }
    }
}