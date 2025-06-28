using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface IBattleStatsDtoFactory
{
    Task<BattleStatsDto> Create(BattleStatsEntity entity);
    Task<UnitBattleStatsDto> CreateUnitBattleStatsDto(UnitBattleStatsEntity entity);
}
