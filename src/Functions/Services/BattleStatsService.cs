using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services;

public interface IBattleStatsService
{
    Task AddAsync(IEnumerable<(string worldId, BattleStats battleStats)> battleStatsItems);
}

public class BattleStatsService(IFogDbContext context, IMapper mapper, ILogger<BattleStatsService> logger)
    : IBattleStatsService
{
    public async Task AddAsync(IEnumerable<(string worldId, BattleStats battleStats)> battleStatsItems)
    {
        var unique = battleStatsItems
            .DistinctBy(t => t.battleStats.BattleId, StructuralByteArrayComparer.Instance)
            .ToDictionary(t => t.battleStats.BattleId, t => t.battleStats, StructuralByteArrayComparer.Instance);
        logger.LogInformation("{ValidCount} unique battles stats after filtering", unique.Count);

        if (unique.Count == 0)
        {
            return;
        }

        var uniqueIdsSet = unique.Keys.ToHashSet(StructuralByteArrayComparer.Instance);
        var existingIds = await GetExistingBattleStatsAsync(uniqueIdsSet);
        logger.LogInformation("Retrieved {ExistingIdsCount} existing battle stats", existingIds.Count);
        var newIds = uniqueIdsSet.Except(existingIds, StructuralByteArrayComparer.Instance)
            .ToHashSet(StructuralByteArrayComparer.Instance);
        logger.LogInformation("{NewIdsCount} new battle stats identified", newIds.Count);

        var newStats = newIds.Select(id =>
            {
                var battleStats = unique[id];
                var playerSquads = battleStats.PlayerSquads.SelectMany(src =>
                {
                    if (src.Hero == null || src.Hero?.SubValues.Count == 0)
                    {
                        return
                        [
                            new BattleSquadStatsEntity
                            {
                                Side = BattleSquadSide.Player,
                                Hero = mapper.Map<UnitBattleStatsEntity>(src.Hero),
                                SupportUnit = mapper.Map<UnitBattleStatsEntity>(src.SupportUnit),
                            },
                        ];
                    }

                    return src.Hero!.SubValues.Select(sv => new BattleSquadStatsEntity
                    {
                        Side = BattleSquadSide.Player,
                        SupportUnit = mapper.Map<UnitBattleStatsEntity>(sv),
                    });
                });
                var enemySquads = battleStats.EnemySquads.SelectMany(src =>
                {
                    if (src.Hero == null || src.Hero?.SubValues.Count == 0)
                    {
                        return
                        [
                            new BattleSquadStatsEntity
                            {
                                Side = BattleSquadSide.Enemy,
                                Hero = mapper.Map<UnitBattleStatsEntity>(src.Hero),
                                SupportUnit = mapper.Map<UnitBattleStatsEntity>(src.SupportUnit),
                            },
                        ];
                    }

                    return src.Hero!.SubValues.Select(sv => new BattleSquadStatsEntity
                    {
                        Side = BattleSquadSide.Enemy,
                        SupportUnit = mapper.Map<UnitBattleStatsEntity>(sv),
                    });
                });
                return new BattleStatsEntity
                {
                    InGameBattleId = id,
                    Squads = playerSquads.Concat(enemySquads).ToList(),
                };
            })
            .Where(src => src.Squads.Count > 0)
            .ToList();
        context.BattleStats.AddRange(newStats);
        await context.SaveChangesAsync();
        logger.LogInformation("SaveChangesAsync completed, added {AddedBattleStatsCount} battle stats", newStats.Count);
    }

    private async Task<HashSet<byte[]>> GetExistingBattleStatsAsync(HashSet<byte[]> inGameBattleIds)
    {
        var existing = await context.BattleStats
            .Where(p => inGameBattleIds.Contains(p.InGameBattleId))
            .Select(p => p.InGameBattleId)
            .ToHashSetAsync(StructuralByteArrayComparer.Instance);
        logger.LogInformation("GetExistingBattleStatsAsync found {Count} items", existing.Count);
        return existing;
    }
}
