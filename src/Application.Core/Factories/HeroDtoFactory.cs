using Ingweland.Fog.Application.Core.Factories.Interfaces;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace Ingweland.Fog.Application.Core.Factories;

public class HeroDtoFactory(IUnitDtoFactory unitDtoFactory,IHohGameLocalizationService localizationService) : IHeroDtoFactory
{
    public HeroDto Create(Hero hero,
        Unit unit,
        IReadOnlyCollection<HeroProgressionCostResource> progressionCosts,
        IReadOnlyDictionary<int, IReadOnlyCollection<ResourceAmount>> ascensionCosts,
        IReadOnlyCollection<UnitStatFormulaData> unitStatCalculationData,
        UnitBattleConstants unitBattleConstants, 
        Unit baseSupportUnit,
        HeroAwakeningComponent awakeningComponent,
        HeroAbilityDto ability, HeroUnitType heroUnitType)
    {
        return new HeroDto
        {
            Id = hero.Id,
            StarClass = hero.ProgressionComponent.Id,
            ProgressionCosts = progressionCosts,
            AscensionCosts = ascensionCosts,
            Unit = unitDtoFactory.Create(unit, unitStatCalculationData, unitBattleConstants, heroUnitType),
            BaseSupportUnit = unitDtoFactory.Create(baseSupportUnit, unitStatCalculationData, unitBattleConstants, heroUnitType),
            AwakeningComponent = awakeningComponent,
            Ability = ability,
            ClassId = hero.ClassId,
            ClassName = localizationService.GetHeroClassName(hero.ClassId.ToString()),
        };
    }
}
