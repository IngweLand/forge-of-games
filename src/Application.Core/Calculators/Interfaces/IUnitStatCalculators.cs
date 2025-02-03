using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace Ingweland.Fog.Application.Core.Calculators.Interfaces;

public interface IUnitStatCalculators
{
    int CalculateHeroStatValueForLevel(int level, int ascensionLevel, float initValue,
        UnitStatFormulaFactors formulaFactors);

    int CalculateUnitStatValueForLevel(int level, float initValue, UnitStatFormulaFactors formulaFactors);

    int CalculateHeroStatValueForLevel(int level, int ascensionLevel, float initValue,
        UnitStatFormulaFactors formulaFactors,
        IReadOnlyCollection<AwakeningLevel> awakeningLevels, float barracksValue);

    float ApplyAwakening(float value, IReadOnlyCollection<AwakeningLevel> awakeningLevels);
    float CalculateAbilityChargeTime(float focusRegeneration, float maxFocus);
    float CalculateAbilityInitialChargeTime(float focusRegeneration, float focus, float maxFocus);
}
