using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
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
    Task AddAsync(string worldId, BattleSummary battleSummary, DateOnly performedAt, Guid? submissionId);
}

public class BattleService(IFogDbContext context, IMapper mapper, ILogger<BattleService> logger) : IBattleService
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = {new JsonStringEnumConverter()},
    };

    public async Task AddAsync(string worldId, BattleSummary battleSummary, DateOnly performedAt, Guid? submissionId)
    {
        var existing = await context.Battles.FirstOrDefaultAsync(x => x.InGameBattleId == battleSummary.BattleId);
        if (existing != null)
        {
            logger.LogInformation(
                "Skipping adding battle. Existing: {ExistingWorldId}:{ExistingBattleId}. New: {NewWorldId}:{NewBattleId}",
                existing.WorldId, Convert.ToBase64String(existing.InGameBattleId), worldId,
                Convert.ToBase64String(battleSummary.BattleId));
            return;
        }

        var allPlayerBattleUnits = battleSummary.PlayerSquads
            .Select(src => src.Hero)
            .Where(srs => srs != null)
            .Select(src => (src!.Properties.UnitId, src.Properties.Level, BattleSquadSide.Player))
            .ToList();
        var allEnemyBattleUnits = battleSummary.EnemySquads
            .Select(src => src.Hero)
            .Where(srs => srs != null)
            .Select(src => (src!.Properties.UnitId, src.Properties.Level, BattleSquadSide.Enemy))
            .ToList();
        var playerBattleUnits = await SaveAndGetBattleUnits(allPlayerBattleUnits);
        var enemyBattleUnits = await SaveAndGetBattleUnits(allEnemyBattleUnits);
        var battleUnits = playerBattleUnits.Concat(enemyBattleUnits).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        var concreteBattlePlayerHeroKeys = battleSummary.PlayerSquads
            .Where(bs => bs.Hero != null)
            .Select(bs => (bs.Hero!.Properties.UnitId, bs.Hero.Properties.Level, BattleSquadSide.Player));
        var concreteBattleEnemyHeroKeys = battleSummary.EnemySquads
            .Where(bs => bs.Hero != null)
            .Select(bs => (bs.Hero!.Properties.UnitId, bs.Hero.Properties.Level, BattleSquadSide.Enemy));
        var concreteBattleHeroKeys = concreteBattlePlayerHeroKeys.Concat(concreteBattleEnemyHeroKeys);
        var concreteBattleHeroes = concreteBattleHeroKeys.Select(t => battleUnits[t]).ToList();
        var difficulty = Difficulty.Undefined;
        if (battleSummary.Location is IBattleLocationWithDifficulty locationWithDifficulty)
        {
            difficulty = locationWithDifficulty.Difficulty;
        }

        const string prefix = "hero_battle.";
        var battleDefinitionId = battleSummary.BattleDefinitionId;
        if (battleDefinitionId.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
        {
            battleDefinitionId = battleDefinitionId[prefix.Length..];
        }

        var squads = new List<BattleSquadsEntity>
        {
            new()
            {
                Squads = JsonSerializer.Serialize(battleSummary.PlayerSquads, JsonSerializerOptions),
                Side = BattleSquadSide.Player,
            },
            new()
            {
                Squads = JsonSerializer.Serialize(battleSummary.EnemySquads, JsonSerializerOptions),
                Side = BattleSquadSide.Enemy,
            },
        };
        var newBattle = new BattleSummaryEntity
        {
            WorldId = worldId,
            InGameBattleId = battleSummary.BattleId,
            BattleDefinitionId = battleDefinitionId,
            Units = concreteBattleHeroes,
            ResultStatus = battleSummary.ResultStatus,
            Difficulty = difficulty,
            Squads = squads,
            BattleType = battleDefinitionId.ToBattleType(),
            SubmissionId = submissionId,
            PerformedAt = performedAt,
        };

        context.Battles.Add(newBattle);
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            logger.LogError(null, "Error adding battle: {NewWorldId}:{NewBattleId}", worldId,
                Convert.ToBase64String(battleSummary.BattleId));
            throw;
        }
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
