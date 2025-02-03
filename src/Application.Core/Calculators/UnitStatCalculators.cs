using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace Ingweland.Fog.Application.Core.Calculators;

// There seems to be inconsistency how the original game handles rounding of stats.
// Hero stats are rounded to the ceiling, while unit stats are rounded to the floor.
public class UnitStatCalculators : IUnitStatCalculators
{
    public int CalculateHeroStatValueForLevel(int level, int ascensionLevel, float initValue,
        UnitStatFormulaFactors formulaFactors)
    {
        if (level < 2)
        {
            return (int) Math.Ceiling(initValue);
        }

        var valuePerLevel = initValue * formulaFactors.Normal;
        var valueAtLevel = valuePerLevel * (level - 1);
        var ascensionsValue = CalculateAscensionsValue(ascensionLevel, initValue, formulaFactors);
        var roundedValue = (int) Math.Ceiling(valueAtLevel + ascensionsValue + initValue);
        return roundedValue;
    }

    public int CalculateHeroStatValueForLevel(int level, int ascensionLevel, float initValue,
        UnitStatFormulaFactors formulaFactors,
        IReadOnlyCollection<AwakeningLevel> awakeningLevels, float barracksValue)
    {
        var heroValue = CalculateHeroStatValueForLevel(level, ascensionLevel, initValue, formulaFactors);
        var awakeningAdjustedValue = ApplyAwakening(heroValue, awakeningLevels);
        return (int) Math.Ceiling(awakeningAdjustedValue + barracksValue);
    }
    
    public float CalculateAbilityChargeTime(float focusRegeneration, float maxFocus)
    {
        return maxFocus / focusRegeneration;
    }
    
    public float CalculateAbilityInitialChargeTime(float focusRegeneration, float focus, float maxFocus)
    {
        var regularTime = CalculateAbilityChargeTime(focusRegeneration, maxFocus);
        return regularTime - focus / focusRegeneration;
    } 

    public int CalculateUnitStatValueForLevel(int level, float initValue, UnitStatFormulaFactors formulaFactors)
    {
        if (level < 2)
        {
            return (int) initValue;
        }

        var valuePerLevel = initValue * formulaFactors.Normal;
        var valueAtLevel = valuePerLevel * (level - 1);
        var roundedValue = (int) (valueAtLevel + initValue);
        return roundedValue;
    }

    public float ApplyAwakening(float value, IReadOnlyCollection<AwakeningLevel> awakeningLevels)
    {
        var percentageValue = awakeningLevels.Where(al => al.IsPercentage).Sum(al => al.Value);
        var absoluteValue = awakeningLevels.Where(al => !al.IsPercentage).Sum(al => al.Value);
        if (percentageValue > 0)
        {
            value += value * percentageValue;
        }

        value += absoluteValue;

        return value;
    }

    private float CalculateAscensionsValue(int ascensionLevel, float initValue, UnitStatFormulaFactors formulaFactors)
    {
        if (ascensionLevel < 1)
        {
            return 0;
        }

        var valuePerLevel = initValue * formulaFactors.Ascension;
        var valueAtLevel = valuePerLevel * ascensionLevel;
        return valueAtLevel;
    }
}
