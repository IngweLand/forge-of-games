using Ingweland.Fog.Application.Core.Interfaces;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Shared.Helpers;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Factories;

public class BattleStatsDtoFactory(
    IHohCoreDataRepository coreDataRepository,
    IHohGameLocalizationService localizationService,
    ILogger<BattleStatsDtoFactory> logger) : IBattleStatsDtoFactory
{
    public async Task<BattleStatsDto> Create(BattleStatsEntity entity)
    {
        var playerSquads = new List<BattleSquadStatsDto>();
        foreach (var squad in entity.PlayerSquads)
        {
            playerSquads.Add(new BattleSquadStatsDto
            {
                Hero = squad.Hero != null ? await CreateUnitBattleStatsDto(squad.Hero) : null,
                SupportUnit = squad.SupportUnit != null ? await CreateUnitBattleStatsDto(squad.SupportUnit) : null,
            });
        }
        
        var enemySquads = new List<BattleSquadStatsDto>();
        foreach (var squad in entity.EnemySquads)
        {
            enemySquads.Add(new BattleSquadStatsDto
            {
                Hero = squad.Hero != null ? await CreateUnitBattleStatsDto(squad.Hero) : null,
                SupportUnit = squad.SupportUnit != null ? await CreateUnitBattleStatsDto(squad.SupportUnit) : null,
            });
        }

        return new BattleStatsDto
        {
            Id = entity.Id,
            PlayerSquads = playerSquads,
            EnemySquads = enemySquads,
        };
    }

    public async Task<UnitBattleStatsDto> CreateUnitBattleStatsDto(UnitBattleStatsEntity entity)
    {
        var unit = await coreDataRepository.GetUnitAsync(entity.UnitId);
        return new UnitBattleStatsDto
        {
            Attack = entity.Attack,
            Defense = entity.Defense,
            Heal = entity.Heal,
            UnitId = entity.UnitId,
            Name = localizationService.GetUnitName(HohStringParser.GetConcreteId(unit!.Id)),
            AssetId = unit.Name,
        };
    }
}
