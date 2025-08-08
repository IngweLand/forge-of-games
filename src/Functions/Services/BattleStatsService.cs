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
    Task AddAsync(IEnumerable<BattleStats> battleStatsItems);
    Task AddAsync(BattleStats battleStats);
}

public class BattleStatsService(IFogDbContext context, IMapper mapper, ILogger<BattleStatsService> logger)
    : IBattleStatsService
{
    public async Task AddAsync(IEnumerable<BattleStats> battleStatsItems)
    {
        var unique = battleStatsItems
            .DistinctBy(x => x.BattleId, StructuralByteArrayComparer.Instance)
            .ToDictionary(x => x.BattleId, x => x, StructuralByteArrayComparer.Instance);
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
                return Create(battleStats);
            })
            .Where(src => src.Squads.Count > 0)
            .ToList();

        foreach (var chunk in newStats.Chunk(25))
        {
            logger.LogInformation("Saving chunk of battle stats with {ChunkSize} items", chunk.Length);
            context.BattleStats.AddRange(chunk);
            await context.SaveChangesAsync();
        }

        logger.LogInformation("SaveChangesAsync completed, added {AddedBattleStatsCount} battle stats", newStats.Count);
    }

    public async Task AddAsync(BattleStats battleStats)
    {
        var existing = await context.BattleStats.AsNoTracking()
            .FirstOrDefaultAsync(x => x.InGameBattleId == battleStats.BattleId);
        var battleIdString = Convert.ToBase64String(battleStats.BattleId);
        if (existing != null)
        {
            logger.LogInformation("BattleStats with id {BattleId} already exists, skipping", battleIdString);
            return;
        }

        var stats = Create(battleStats);
        context.BattleStats.Add(stats);
        await context.SaveChangesAsync();
        logger.LogInformation("BattleStats with id {BattleId} added", battleIdString);
    }

    private BattleStatsEntity Create(BattleStats battleStats)
    {
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

            var l = new List<BattleSquadStatsEntity>
            {
                new()
                {
                    Side = BattleSquadSide.Player,
                    Hero = mapper.Map<UnitBattleStatsEntity>(src.Hero),
                    SupportUnit = mapper.Map<UnitBattleStatsEntity>(src.SupportUnit),
                },
            };
            l.AddRange(src.Hero!.SubValues.Select(sv => new BattleSquadStatsEntity
            {
                Side = BattleSquadSide.Player,
                SupportUnit = mapper.Map<UnitBattleStatsEntity>(sv),
            }));
            return l;
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

            var l = new List<BattleSquadStatsEntity>
            {
                new()
                {
                    Side = BattleSquadSide.Enemy,
                    Hero = mapper.Map<UnitBattleStatsEntity>(src.Hero),
                    SupportUnit = mapper.Map<UnitBattleStatsEntity>(src.SupportUnit),
                },
            };
            l.AddRange(src.Hero!.SubValues.Select(sv => new BattleSquadStatsEntity
            {
                Side = BattleSquadSide.Enemy,
                SupportUnit = mapper.Map<UnitBattleStatsEntity>(sv),
            }));
            return l;
        });
        return new BattleStatsEntity
        {
            InGameBattleId = battleStats.BattleId,
            Squads = playerSquads.Concat(enemySquads).ToList(),
        };
    }

    private async Task<HashSet<byte[]>> GetExistingBattleStatsAsync(HashSet<byte[]> inGameBattleIds)
    {
        var existing = await context.BattleStats.AsNoTracking()
            .Where(p => inGameBattleIds.Contains(p.InGameBattleId))
            .Select(p => p.InGameBattleId)
            .ToHashSetAsync(StructuralByteArrayComparer.Instance);
        logger.LogInformation("GetExistingBattleStatsAsync found {Count} items", existing.Count);
        return existing;
    }
}
