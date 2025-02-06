using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface IHeroDtoFactory
{
    HeroDto Create(Hero hero,
        Unit unit,
        IReadOnlyCollection<HeroProgressionCostResource> progressionCosts,
        IReadOnlyDictionary<int, IReadOnlyCollection<ResourceAmount>> ascensionCosts,
        IReadOnlyCollection<UnitStatFormulaData> unitStatCalculationData,
        UnitBattleConstants unitBattleConstants, 
        Unit baseSupportUnit,
        HeroAwakeningComponent awakeningComponent,
        BattleAbilityDto ability, HeroUnitType heroUnitType);
}
