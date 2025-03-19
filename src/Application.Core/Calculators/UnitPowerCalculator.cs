using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.Calculators;

public class UnitPowerCalculator(IUnitStatCalculators unitStatCalculators) : IUnitPowerCalculator
{
    public double CalculateUnitPower(UnitDto unit, int level, int squadSize)
    {
        var unitPower = Calculate(
            attack: GetUnitValue(UnitStatType.Attack, level, unit),
            defense: GetUnitValue(UnitStatType.Defense, level, unit),
            maxHitPoints: GetUnitValue(UnitStatType.MaxHitPoints, level, unit),
            baseDamage: GetUnitValue(UnitStatType.BaseDamage, level, unit),
            expectedSquadSize: unit.Stats.First(us => us.Type == UnitStatType.ExpectedSquadSize).Value,
            attackSpeed: unit.Stats.First(us => us.Type == UnitStatType.AttackSpeed).Value,
            attackRange: unit.Stats.First(us => us.Type == UnitStatType.AttackRange).Value,
            rarity: 0,
            abilityLevel: 0,
            squadSize: squadSize,
            evasion: unit.Stats.First(us => us.Type == UnitStatType.Evasion).Value,
            focusRegen: unit.Stats.First(us => us.Type == UnitStatType.FocusRegen).Value,
            critChance: unit.Stats.First(us => us.Type == UnitStatType.FocusRegen).Value,
            critDamage: unit.Stats.First(us => us.Type == UnitStatType.FocusRegen).Value,
            unboostedFocusRegen: 1
        );

        return unitPower;
    }

    public double CalculateUnitPower(IReadOnlyDictionary<UnitStatType, float> stats)
    {
        var unitPower = Calculate(
            attack: stats[UnitStatType.Attack],
            defense: stats[UnitStatType.Defense],
            maxHitPoints: stats[UnitStatType.MaxHitPoints],
            baseDamage: stats[UnitStatType.BaseDamage],
            expectedSquadSize: stats[UnitStatType.ExpectedSquadSize],
            attackSpeed: stats[UnitStatType.AttackSpeed],
            attackRange: stats[UnitStatType.AttackRange],
            rarity: 0,
            abilityLevel: 1,
            squadSize: stats[UnitStatType.SquadSize],
            evasion: stats[UnitStatType.Evasion],
            focusRegen: stats[UnitStatType.FocusRegen],
            critChance: stats[UnitStatType.CritChance],
            critDamage: stats[UnitStatType.CritDamage],
            unboostedFocusRegen: 1
        );

        return unitPower;
    }

    public double CalculateHeroPower(UnitDto unit, HeroStarClass rarity, int level, int ascensionLevel,
        int abilityLevel)
    {
        var unitPower = Calculate(
            attack: GetHeroValue(UnitStatType.Attack, level, ascensionLevel, unit),
            defense: GetHeroValue(UnitStatType.Defense, level, ascensionLevel, unit),
            maxHitPoints: GetHeroValue(UnitStatType.MaxHitPoints, level, ascensionLevel, unit),
            baseDamage: GetHeroValue(UnitStatType.BaseDamage, level, ascensionLevel, unit),
            expectedSquadSize: 1,
            attackSpeed: unit.Stats.First(us => us.Type == UnitStatType.AttackSpeed).Value,
            attackRange: unit.Stats.First(us => us.Type == UnitStatType.AttackRange).Value,
            rarity: rarity,
            abilityLevel: abilityLevel,
            squadSize: 1,
            evasion: unit.Stats.First(us => us.Type == UnitStatType.Evasion).Value,
            focusRegen: unit.Stats.First(us => us.Type == UnitStatType.FocusRegen).Value,
            critChance: unit.Stats.First(us => us.Type == UnitStatType.CritChance).Value,
            critDamage: unit.Stats.First(us => us.Type == UnitStatType.CritDamage).Value,
            unboostedFocusRegen: unit.Stats.First(us => us.Type == UnitStatType.FocusRegen).Value
        );

        return unitPower;
    }

    public double CalculateHeroPower(IReadOnlyDictionary<UnitStatType, float> stats, HeroStarClass rarity,
        int abilityLevel, IReadOnlyDictionary<UnitStatType, float> unboostedStats)
    {
        var unitPower = Calculate(
            attack: stats[UnitStatType.Attack],
            defense: stats[UnitStatType.Defense],
            maxHitPoints: stats[UnitStatType.MaxHitPoints],
            baseDamage: stats[UnitStatType.BaseDamage],
            expectedSquadSize: 1,
            attackSpeed: stats[UnitStatType.AttackSpeed],
            attackRange: stats[UnitStatType.AttackRange],
            rarity: rarity,
            abilityLevel: abilityLevel,
            squadSize: 1,
            evasion: stats[UnitStatType.Evasion],
            focusRegen: stats[UnitStatType.FocusRegen],
            critChance: stats[UnitStatType.CritChance],
            critDamage: stats[UnitStatType.CritDamage],
            unboostedFocusRegen: unboostedStats[UnitStatType.FocusRegen]
        );

        return unitPower;
    }

    private float GetUnitValue(UnitStatType unitStatType, int level, UnitDto unit)
    {
        return unitStatCalculators.CalculateUnitStatValueForLevel(level,
            unit.Stats.First(us => us.Type == unitStatType).Value,
            unit.StatCalculationFactors[unitStatType]);
    }

    private float GetHeroValue(UnitStatType unitStatType, int level, int ascensionLevel, UnitDto unit)
    {
        return unitStatCalculators.CalculateHeroStatValueForLevel(level, ascensionLevel,
            unit.Stats.First(us => us.Type == unitStatType).Value,
            unit.StatCalculationFactors[unitStatType]);
    }

    private double Calculate(
        float attack,
        float defense,
        float maxHitPoints,
        float evasion,
        float baseDamage,
        float critChance,
        float critDamage,
        float expectedSquadSize,
        float attackSpeed,
        float attackRange,
        HeroStarClass rarity,
        int abilityLevel,
        float squadSize,
        float focusRegen,
        float unboostedFocusRegen)
    {
        return Math.Round(
            Math.Sqrt(
                attack *
                defense *
                maxHitPoints *
                (1.0 / (1.0 + (-1.0 * evasion))) *
                baseDamage *
                (1.0 + (critChance * (critDamage - 1.0))) *
                (0.5 + 0.5 / expectedSquadSize) *
                (
                    attackSpeed * (1.0 + 0.0168 * (attackRange - 1.25)) +
                    ((GetRarityMultiplier(rarity) + ((abilityLevel - 1.0) * 0.025)) * (focusRegen / unboostedFocusRegen))
                )
            ) * 
            0.001218 *
            squadSize
        );
    }

    private static double GetRarityMultiplier(HeroStarClass rarity)
    {
        return rarity switch
        {
            HeroStarClass.Star_2 => 0.9,
            HeroStarClass.Star_3 => 1.35,
            HeroStarClass.Star_4 => 1.75,
            HeroStarClass.Star_5 => 2.03,
            _ => 0,
        };
    }
}
