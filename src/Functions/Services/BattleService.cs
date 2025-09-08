using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services;

public interface IBattleService
{
    Task AddAsync(
        IEnumerable<(string WorldId, BattleSummary BattleSummary, DateOnly PerformedAt, Guid? SubmissionId)> battles);

    Task AddAsync(string worldId, BattleSummary battleSummary, DateOnly performedAt, Guid? submissionId);
}

public class BattleService(IFogDbContext context, IMapper mapper, ILogger<BattleService> logger) : IBattleService
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = {new JsonStringEnumConverter()},
    };

    public Task AddAsync(string worldId, BattleSummary battleSummary, DateOnly performedAt, Guid? submissionId)
    {
        return AddAsync([
            (WorldId: worldId, BattleSummary: battleSummary, PerformedAt: performedAt, SubmissionId: submissionId),
        ]);
    }

    public async Task AddAsync(
        IEnumerable<(string WorldId, BattleSummary BattleSummary, DateOnly PerformedAt, Guid? SubmissionId)> battles)
    {
        var unique = battles
            .DistinctBy(t => new BattleKey(t.WorldId, t.BattleSummary.BattleId))
            .ToDictionary(t => new BattleKey(t.WorldId, t.BattleSummary.BattleId),
                t => (t.BattleSummary, SubmittedAt: t.PerformedAt, t.SubmissionId));
        logger.LogInformation("{ValidCount} unique battles after filtering", unique.Count);

        if (unique.Count == 0)
        {
            return;
        }

        var existingBattleKeys =
            await GetExistingBattlesAsync(unique.Keys.Select(bk => bk.InGameBattleId).ToHashSet());
        logger.LogInformation("Retrieved {ExistingBattlesCount} existing battles", existingBattleKeys.Count);
        var newBattleKeys = unique.Keys.ToHashSet().Except(existingBattleKeys).ToHashSet();
        logger.LogInformation("{NewBattlesCount} new battles identified", newBattleKeys.Count);
        List<KeyValuePair<BattleKey, (BattleSummary BattleSummary, DateOnly PerformedAt, Guid? SubmissionId)>>?
            newBattlesData = unique.Where(kvp => newBattleKeys.Contains(kvp.Key)).ToList();

        var allPlayerBattleUnits = newBattlesData
            .SelectMany(kvp => kvp.Value.BattleSummary.PlayerSquads.Select(src => src.Hero))
            .Where(srs => srs != null)
            .Select(src => (src!.Properties.UnitId, src.Properties.Level, BattleSquadSide.Player))
            .ToList();
        var allEnemyBattleUnits = newBattlesData
            .SelectMany(kvp => kvp.Value.BattleSummary.EnemySquads.Select(src => src.Hero))
            .Where(srs => srs != null)
            .Select(src => (src!.Properties.UnitId, src.Properties.Level, BattleSquadSide.Enemy))
            .ToList();
        var playerBattleUnits = await SaveAndGetBattleUnits(allPlayerBattleUnits);
        var enemyBattleUnits = await SaveAndGetBattleUnits(allEnemyBattleUnits);
        var battleUnits = playerBattleUnits.Concat(enemyBattleUnits).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        var newBattles = newBattlesData.Select(src =>
            {
                var concreteBattlePlayerHeroKeys = src.Value.BattleSummary.PlayerSquads
                    .Where(bs => bs.Hero != null)
                    .Select(bs => (bs.Hero!.Properties.UnitId, bs.Hero.Properties.Level, BattleSquadSide.Player));
                var concreteBattleEnemyHeroKeys = src.Value.BattleSummary.EnemySquads
                    .Where(bs => bs.Hero != null)
                    .Select(bs => (bs.Hero!.Properties.UnitId, bs.Hero.Properties.Level, BattleSquadSide.Enemy));
                var concreteBattleHeroKeys = concreteBattlePlayerHeroKeys.Concat(concreteBattleEnemyHeroKeys);
                var concreteBattleHeroes = concreteBattleHeroKeys.Select(t => battleUnits[t]).ToList();
                var difficulty = Difficulty.Undefined;
                if (src.Value.BattleSummary.Location is IBattleLocationWithDifficulty locationWithDifficulty)
                {
                    difficulty = locationWithDifficulty.Difficulty;
                }

                const string prefix = "hero_battle.";
                var battleDefinitionId = src.Value.BattleSummary.BattleDefinitionId;
                if (battleDefinitionId.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                {
                    battleDefinitionId = battleDefinitionId[prefix.Length..];
                }

                var squads = new List<BattleSquadsEntity>()
                    {
                    new BattleSquadsEntity
                    {
                        Squads = JsonSerializer.Serialize(src.Value.BattleSummary.PlayerSquads, JsonSerializerOptions),
                        Side = BattleSquadSide.Player,
                    },
                    new BattleSquadsEntity
                    {
                        Squads = JsonSerializer.Serialize(src.Value.BattleSummary.EnemySquads, JsonSerializerOptions),
                        Side = BattleSquadSide.Enemy,
                    }
                };
                return new BattleSummaryEntity
                {
                    WorldId = src.Key.WorldId,
                    InGameBattleId = src.Key.InGameBattleId,
                    BattleDefinitionId = battleDefinitionId,
                    Units = concreteBattleHeroes,
                    ResultStatus = src.Value.BattleSummary.ResultStatus,
                    Difficulty = difficulty,
                    Squads = squads,
                    BattleType = battleDefinitionId.ToBattleType(),
                    SubmissionId = src.Value.SubmissionId,
                    PerformedAt = src.Value.PerformedAt,
                };
            })
            .ToList();

        foreach (var chunk in newBattles.Chunk(50))
        {
            logger.LogInformation("Processing chunk of battles with {ChunkSize} items", chunk.Length);
            context.Battles.AddRange(chunk);
            await context.SaveChangesAsync();
        }

        logger.LogInformation("SaveChangesAsync completed, added {AddedBattlesCount} battles", newBattles.Count);
    }

    private async Task<HashSet<BattleKey>> GetExistingBattlesAsync(HashSet<byte[]> inGameBattleIds)
    {
        var existing = await context.Battles
            .Where(p => inGameBattleIds.Contains(p.InGameBattleId))
            .ProjectTo<BattleKey>(mapper.ConfigurationProvider)
            .ToHashSetAsync();
        logger.LogInformation("GetExistingBattlesAsync found {Count} battles", existing.Count);
        return existing;
    }

    private async Task<Dictionary<(string UnitId, int Level, BattleSquadSide Side), BattleUnitEntity>>
        SaveAndGetBattleUnits(
            List<(string UnitId, int Level, BattleSquadSide Side)> battleUnitTuples)
    {
        if (battleUnitTuples.Count == 0)
        {
            return [];
        }

        var unitLevelPairs = battleUnitTuples.Distinct().ToList();
        var unitIds = unitLevelPairs.Select(p => p.UnitId).Distinct().ToList();

        var candidateBattleHeroes = await context.BattleUnits
            .Where(bh => unitIds.Contains(bh.UnitId))
            .ToListAsync();
        var existingKeys = candidateBattleHeroes
            .Where(bh => unitLevelPairs.Contains((bh.UnitId, bh.Level, bh.Side)))
            .Select(bh => (bh.UnitId, bh.Level, bh.Side)).ToHashSet();

        var newBattleUnits = unitLevelPairs
            .Where(pair => !existingKeys.Contains((pair.UnitId, pair.Level, pair.Side)))
            .Select(pair => new BattleUnitEntity
            {
                UnitId = pair.UnitId,
                Level = pair.Level,
                Side = pair.Side,
            })
            .ToList();

        if (newBattleUnits.Count != 0)
        {
            context.BattleUnits.AddRange(newBattleUnits);
            await context.SaveChangesAsync();
        }

        var allBattleUnitCandidates = await context.BattleUnits
            .Where(bh => unitIds.Contains(bh.UnitId))
            .ToListAsync();

        return allBattleUnitCandidates
            .Where(bh => unitLevelPairs.Contains((bh.UnitId, bh.Level, bh.Side)))
            .ToDictionary(bh => (bh.UnitId, bh.Level, bh.Side));
    }
}
