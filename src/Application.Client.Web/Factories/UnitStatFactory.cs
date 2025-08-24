using System.Collections.ObjectModel;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class UnitStatFactory(IUnitStatCalculators statCalculators) : IUnitStatFactory
{
    public IReadOnlyDictionary<UnitStatType, float> CreateHeroStats(HeroDto hero,
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
                    stats.Add(unitStat.Type,
                        CreateHeroLevelStat(unitStat.Type, hero, level, ascensionLevel, awakeningLevel, barracks));
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
        var stats = new Dictionary<UnitStatType, float>
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

    public IReadOnlyDictionary<UnitStatType, IReadOnlyDictionary<UnitStatSource, float>> CreateHeroStats(
        HeroDto hero, int level, int ascensionLevel, IReadOnlyCollection<StatBoost> boosts, BuildingDto? barracks)
    {
        var stats = new Dictionary<UnitStatType, IReadOnlyDictionary<UnitStatSource, float>>();
        foreach (var unitStat in hero.Unit.Stats)
        {
            var values = CreateHeroStats(unitStat, hero, level, ascensionLevel, boosts, barracks);
            stats[unitStat.Type] = new ReadOnlyDictionary<UnitStatSource, float>(values.ToDictionary());
        }

        return new ReadOnlyDictionary<UnitStatType, IReadOnlyDictionary<UnitStatSource, float>>(stats);
    }

    public IReadOnlyDictionary<UnitStatSource, float> CreateHeroStats(UnitStat unitStat, HeroDto hero, int level,
        int ascensionLevel, IReadOnlyCollection<StatBoost> boosts, BuildingDto? barracks)
    {
        float unitStatValue;
        switch (unitStat.Type)
        {
            case UnitStatType.Attack:
            case UnitStatType.Defense:
            case UnitStatType.MaxHitPoints:
            case UnitStatType.BaseDamage:
            {
                unitStatValue = CreateHeroLevelStat(unitStat.Type, hero, level, ascensionLevel, 0, null);
                break;
            }
            default:
            {
                unitStatValue = hero.Unit.Stats.First(us => us.Type == unitStat.Type).Value;
                break;
            }
        }

        var values = new Dictionary<UnitStatSource, float> {{UnitStatSource.Base, unitStatValue}};

        var unitStatBoosts = boosts.Where(x => x.UnitStatType == unitStat.Type).OrderBy(x => x.Order);
        foreach (var boost in unitStatBoosts)
        {
            var boostedValue = 0.0f;
            switch (boost.Calculation)
            {
                case Calculation.Add:
                    boostedValue = boost.Value;
                    break;
                case Calculation.Multiply:
                    boostedValue = unitStatValue * boost.Value;
                    break;
            }

            if (boost.StatAttribute != null)
            {
                values.TryAdd(UnitStatSource.Equipment, 0);

                values[UnitStatSource.Equipment] += boostedValue;
            }
            else
            {
                values.TryAdd(UnitStatSource.Unknown, 0);

                values[UnitStatSource.Unknown] += boostedValue;
            }
        }

        var barracksStat = barracks?.Components
            .OfType<HeroBuildingBoostComponent>()
            .FirstOrDefault()
            ?.UnitStats
            .FirstOrDefault(us => us.Type == unitStat.Type);
        if (barracksStat != null)
        {
            values[UnitStatSource.Barracks] = barracksStat.Value;
            var adjusted = values[UnitStatSource.Unknown] - barracksStat.Value;
            if (adjusted == 0)
            {
                values.Remove(UnitStatSource.Unknown);
            }
            else
            {
                values[UnitStatSource.Unknown] = adjusted;
            }
        }

        return values;
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

    private IReadOnlyCollection<AwakeningLevel> GetAwakeningLevels(UnitStatType unitStatType, HeroDto hero,
        int awakeningLevel)
    {
        return hero.AwakeningComponent.Levels.Take(awakeningLevel)
            .Where(al => al.StatType == unitStatType).ToList();
    }
}
