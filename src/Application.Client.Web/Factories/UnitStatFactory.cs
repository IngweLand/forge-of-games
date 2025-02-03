using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class UnitStatFactory(IUnitStatCalculators statCalculators) : IUnitStatFactory
{
    public IReadOnlyDictionary<UnitStatType, float> CreateMainHeroStats(HeroDto hero,
        int level, int ascensionLevel, int awakeningLevel, BuildingDto? barracks)
    {
       var stats = new Dictionary<UnitStatType, float>();
        foreach (var unitStat in hero.Unit.Stats)
        {
            switch (unitStat.Type)
            {
                case UnitStatType.Attack:
                case UnitStatType.Defense:
                case UnitStatType.MaxHitPoints:
                case UnitStatType.BaseDamage:
                {
                    stats.Add(unitStat.Type,  CreateHeroLevelStat(unitStat.Type, hero, level,ascensionLevel, awakeningLevel, barracks));
                    break;
                }
                default:
                {
                    stats.Add(unitStat.Type, ApplyAwakening(unitStat.Type, hero, awakeningLevel));
                    break;
                }
            }
        }
            
        return stats;
    }

    public IReadOnlyDictionary<UnitStatType, float> CreateMainSupportUnitStats(IUnit unit, int level,
        IReadOnlyDictionary<UnitStatType, UnitStatFormulaFactors> statCalculationFactors)
    {
        var stats = new Dictionary<UnitStatType, float>()
        {
            {
                UnitStatType.Attack,
                CreateSupportUnitLevelStat(UnitStatType.Attack, unit, level, statCalculationFactors)
            },
            {
                UnitStatType.Defense,
                CreateSupportUnitLevelStat(UnitStatType.Defense, unit, level, statCalculationFactors)
            },
            {
                UnitStatType.MaxHitPoints,
                CreateSupportUnitLevelStat(UnitStatType.MaxHitPoints, unit, level, statCalculationFactors)
            },
            {
                UnitStatType.BaseDamage,
                CreateSupportUnitLevelStat(UnitStatType.BaseDamage, unit, level, statCalculationFactors)
            },
        };
        return stats;
    }

    private float CreateSupportUnitLevelStat(UnitStatType unitStatType, IUnit unit, int level,
        IReadOnlyDictionary<UnitStatType, UnitStatFormulaFactors> statCalculationFactors)
    {
        return statCalculators.CalculateUnitStatValueForLevel(level,
            unit.Stats.First(us => us.Type == unitStatType).Value,
            statCalculationFactors[unitStatType]);
    }

    private float CreateHeroLevelStat(UnitStatType unitStatType, HeroDto hero,
        int level, int ascensionLevel, int awakeningLevel, BuildingDto? barracks)
    {
        var barracksValue = 0f;
        var boostStat = barracks?.Components
            .OfType<HeroBuildingBoostComponent>()
            .FirstOrDefault()
            ?.UnitStats
            .FirstOrDefault(us => us.Type == unitStatType);
        if (boostStat != null)
        {
            barracksValue = boostStat.Value;
        }

        var awakeningLevels = GetAwakeningLevels(unitStatType, hero, awakeningLevel);

        return statCalculators.CalculateHeroStatValueForLevel(level, ascensionLevel,
            hero.Unit.Stats.First(us => us.Type == unitStatType).Value,
            hero.Unit.StatCalculationFactors[unitStatType], awakeningLevels, barracksValue);
    }
    
    private float ApplyAwakening(UnitStatType unitStatType, HeroDto hero, int awakeningLevel)
    {
        var awakeningLevels = GetAwakeningLevels(unitStatType, hero, awakeningLevel);
        var value = hero.Unit.Stats.First(us => us.Type == unitStatType).Value;
        return awakeningLevels.Count == 0 ? value : statCalculators.ApplyAwakening(value, awakeningLevels);
    }
    
    private IReadOnlyCollection<AwakeningLevel> GetAwakeningLevels(UnitStatType unitStatType, HeroDto hero, int awakeningLevel)
    {
        return hero.AwakeningComponent.Levels.Take(awakeningLevel)
            .Where(al => al.StatType == unitStatType).ToList();
    }
}
