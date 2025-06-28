using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface IUnitBattleDtoFactory
{
    Task<IReadOnlyCollection<UnitBattleDto>> CreateUnitBattles(IReadOnlyCollection<BattleSummaryEntity> battles,
        string targetUnitId, IReadOnlyDictionary<byte[], BattleStatsEntity> battleStatsDic);
}
