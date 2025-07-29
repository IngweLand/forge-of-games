using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Functions.Constants;
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
    Task AddAsync(IEnumerable<(string WorldId, BattleSummary BattleSummary)> battles);
}

public class BattleService(IFogDbContext context, IMapper mapper, ILogger<BattleService> logger) : IBattleService
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = {new JsonStringEnumConverter()},
    };

    public async Task AddAsync(IEnumerable<(string WorldId, BattleSummary BattleSummary)> battles)
    {
        var unique = battles
            .DistinctBy(t => new BattleKey(t.WorldId, t.BattleSummary.BattleId))
            .ToDictionary(t => new BattleKey(t.WorldId, t.BattleSummary.BattleId), t => t.BattleSummary);
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
        var newBattlesData = unique.Where(kvp => newBattleKeys.Contains(kvp.Key)).ToList();

        var allPlayerBattleUnits = newBattlesData
            .SelectMany(kvp => kvp.Value.PlayerSquads.Select(src => src.Hero))
            .Where(srs => srs != null)
            .Select(src => (src!.Properties.UnitId, src.Properties.Level, BattleSquadSide.Player))
            .ToList();
        var allEnemyBattleUnits = newBattlesData
            .SelectMany(kvp => kvp.Value.EnemySquads.Select(src => src.Hero))
            .Where(srs => srs != null)
            .Select(src => (src!.Properties.UnitId, src.Properties.Level, BattleSquadSide.Enemy))
            .ToList();
        var playerBattleUnits = await SaveAndGetBattleUnits(allPlayerBattleUnits);
        var enemyBattleUnits = await SaveAndGetBattleUnits(allEnemyBattleUnits);
        var battleUnits = playerBattleUnits.Concat(enemyBattleUnits).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        var newBattles = newBattlesData.Select(src =>
            {
                var concreteBattlePlayerHeroKeys = src.Value.PlayerSquads
                    .Where(bs => bs.Hero != null)
                    .Select(bs => (bs.Hero!.Properties.UnitId, bs.Hero.Properties.Level, BattleSquadSide.Player));
                var concreteBattleEnemyHeroKeys = src.Value.EnemySquads
                    .Where(bs => bs.Hero != null)
                    .Select(bs => (bs.Hero!.Properties.UnitId, bs.Hero.Properties.Level, BattleSquadSide.Enemy));
                var concreteBattleHeroKeys = concreteBattlePlayerHeroKeys.Concat(concreteBattleEnemyHeroKeys);
                var concreteBattleHeroes = concreteBattleHeroKeys.Select(t => battleUnits[t]).ToList();
                var difficulty = Difficulty.Undefined;
                if (src.Value.Location is IBattleLocationWithDifficulty locationWithDifficulty)
                {
                    difficulty = locationWithDifficulty.Difficulty;
                }

                const string prefix = "hero_battle.";
                var battleDefinitionId = src.Value.BattleDefinitionId;
                if (battleDefinitionId.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                {
                    battleDefinitionId = battleDefinitionId[prefix.Length..];
                }
                return new BattleSummaryEntity()
                {
                    WorldId = src.Key.WorldId,
                    InGameBattleId = src.Key.InGameBattleId,
                    BattleDefinitionId = battleDefinitionId,
                    Units = concreteBattleHeroes,
                    ResultStatus = src.Value.ResultStatus,
                    Difficulty = difficulty,
                    PlayerSquads = JsonSerializer.Serialize(src.Value.PlayerSquads, JsonSerializerOptions),
                    EnemySquads = JsonSerializer.Serialize(src.Value.EnemySquads, JsonSerializerOptions),
                    BattleType = battleDefinitionId.ToBattleType(),
                };
            })
            .ToList();
        
        foreach (var chunk in newBattles.Chunk(200))
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

    private async Task<Dictionary<(string UnitId, int Level, BattleSquadSide Side), BattleUnitEntity>> SaveAndGetBattleUnits(
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
