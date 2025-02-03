using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.Calculators.Interfaces;

public interface IUnitPowerCalculator
{
    double CalculateUnitPower(UnitDto unit, int level, int squadSize);
    double CalculateHeroPower(UnitDto unit, HeroStarClass rarity, int level,int ascensionLevel, int abilityLevel);

    double CalculateHeroPower(IReadOnlyDictionary<UnitStatType, float> stats, HeroStarClass rarity,
        int abilityLevel, IReadOnlyDictionary<UnitStatType, float> unboostedStats);

    double CalculateUnitPower(IReadOnlyDictionary<UnitStatType, float> stats);
}
