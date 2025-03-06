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
        var existingAllianceKeys =
            await GetExistingAlliancesAsync(unique.Keys.Select(ak => ak.InGameAllianceId).ToHashSet());
        var newAllianceKeys = unique.Keys.ToHashSet().Except(existingAllianceKeys);
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
        });
        context.Alliances.AddRange(newAlliances);
        await context.SaveChangesAsync();
    }

    private async Task<HashSet<AllianceKey>> GetExistingAlliancesAsync(HashSet<int> inGameAllianceIds)
    {
        return await context.Alliances
            .Where(p => inGameAllianceIds.Contains(p.InGameAllianceId))
            .ProjectTo<AllianceKey>(mapper.ConfigurationProvider)
            .ToHashSetAsync();
    }
}