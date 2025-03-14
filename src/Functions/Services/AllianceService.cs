using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Alliance = Ingweland.Fog.Models.Fog.Entities.Alliance;

namespace Ingweland.Fog.Functions.Services;

public interface IAllianceService
{
    Task AddAsync(IEnumerable<AllianceAggregate> allianceAggregates);
}

public class AllianceService(IFogDbContext context, IMapper mapper, ILogger<PlayerRankingService> logger)
    : IAllianceService
{
    public async Task AddAsync(IEnumerable<AllianceAggregate> allianceAggregates)
    {
        var unique = allianceAggregates
            .Where(a => a.CanBeConvertedToAlliance())
            .OrderByDescending(p => p.CollectedAt) // we need this to correctly set UpdateAt on the alliance
            .DistinctBy(p => p.Key)
            .ToDictionary(p => p.Key);
        logger.LogInformation("{ValidCount} valid alliance aggregates after filtering", unique.Count);
        var existingAllianceKeys =
            await GetExistingAlliancesAsync(unique.Keys.Select(ak => ak.InGameAllianceId).ToHashSet());
        logger.LogInformation("Retrieved {ExistingAllianceCount} existing alliances", existingAllianceKeys.Count);
        var newAllianceKeys = unique.Keys.ToHashSet().Except(existingAllianceKeys).ToList();
        logger.LogInformation("{NewAllianceCount} new alliances identified", newAllianceKeys.Count);
        var newAlliances = newAllianceKeys.Select(k =>
        {
            var allianceAggregate = unique[k];
            return new Alliance()
            {
                WorldId = allianceAggregate.WorldId,
                InGameAllianceId = allianceAggregate.InGameAllianceId,
                Name = allianceAggregate.Name,
                AvatarIconId = allianceAggregate.AvatarIconId ?? 0,
                AvatarBackgroundId = allianceAggregate.AvatarBackgroundId ?? 0,
                UpdatedAt = DateOnly.FromDateTime(allianceAggregate.CollectedAt)
            };
        }).ToList();
        context.Alliances.AddRange(newAlliances);
        await context.SaveChangesAsync();
        logger.LogInformation("SaveChangesAsync completed, added {AddedAllianceCount} alliances", newAlliances.Count);
    }

    private async Task<HashSet<AllianceKey>> GetExistingAlliancesAsync(HashSet<int> inGameAllianceIds)
    {
        var existing = await context.Alliances
            .Where(p => inGameAllianceIds.Contains(p.InGameAllianceId))
            .ProjectTo<AllianceKey>(mapper.ConfigurationProvider)
            .ToHashSetAsync();
        logger.LogInformation("GetExistingAlliancesAsync found {Count} alliances", existing.Count);
        return existing;
    }
}