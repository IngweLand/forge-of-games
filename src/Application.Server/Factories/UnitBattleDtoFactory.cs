using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Helpers;

namespace Ingweland.Fog.Application.Server.Factories;

public class UnitBattleDtoFactory(
    IHohGameLocalizationService localizationService,
    IMapper mapper,
    IBattleStatsDtoFactory battleStatsDtoFactory) : IUnitBattleDtoFactory
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = {new JsonStringEnumConverter()},
    };

    public async Task<IReadOnlyCollection<UnitBattleDto>> CreateUnitBattles(
        IReadOnlyCollection<BattleSummaryEntity> battles, string targetUnitId,
        IReadOnlyDictionary<byte[], BattleStatsEntity> battleStatsDic)
    {
        var unitBattles = new List<UnitBattleDto>();
        var unitName = localizationService.GetUnitName(HohStringParser.GetConcreteId(targetUnitId));
        foreach (var battle in battles)
        {
            var unitBattle = await CreateUnitBattleDto(targetUnitId, battleStatsDic, battle, unitName);

            if (unitBattle != null)
            {
                unitBattles.Add(unitBattle);
            }

            if (battle.BattleDefinitionId.ToBattleType() == BattleType.Pvp)
            {
                var enemyUnitBattle = await CreateUnitBattleDto(targetUnitId, battleStatsDic, battle, unitName,
                    BattleSquadSide.Enemy);
                if (enemyUnitBattle != null)
                {
                    unitBattles.Add(enemyUnitBattle);
                }
            }
        }

        return unitBattles;
    }

    private async Task<UnitBattleDto?> CreateUnitBattleDto(string targetUnitId,
        IReadOnlyDictionary<byte[], BattleStatsEntity> battleStatsDic,
        BattleSummaryEntity battle, string unitName, BattleSquadSide side = BattleSquadSide.Player)
    {
        UnitBattleStatsDto? battleStatsDto = null;
        if (battleStatsDic.TryGetValue(battle.InGameBattleId, out var statsEntity))
        {
            var unitBattleStats = statsEntity.Squads.Where(src => src.Side == side)
                .FirstOrDefault(src => src.Hero?.UnitId == targetUnitId)?
                .Hero;
            if (unitBattleStats != null)
            {
                battleStatsDto = await battleStatsDtoFactory.CreateUnitBattleStatsDto(unitBattleStats);
            }
        }

        var serializedSquads = side == BattleSquadSide.Enemy ? battle.EnemySquads : battle.PlayerSquads;
        var playerSquads =
            JsonSerializer.Deserialize<IReadOnlyCollection<BattleSquad>>(serializedSquads, JsonSerializerOptions) ??
            [];
        var battleUnit = playerSquads
            .FirstOrDefault(src => src.Hero != null && src.Hero.Properties.UnitId == targetUnitId)?.Hero;
        if (battleUnit == null)
        {
            return null;
        }

        return new UnitBattleDto
        {
            BattleDefinitionId = battle.BattleDefinitionId,
            UnitName = unitName,
            Unit = mapper.Map<BattleUnitDto>(battleUnit.Properties),
            BattleStats = battleStatsDto,
            Difficulty = battle.Difficulty,
        };
    }
}
