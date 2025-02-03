using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface IUnitDtoFactory
{
    UnitDto Create(Unit unit, IEnumerable<UnitStatFormulaData> unitStatCalculationData,
        UnitBattleConstants unitBattleConstants, HeroUnitType heroUnitType);
}
