using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class HeroProfileFactory(
    IUnitStatFactory unitStatFactory,
    IUnitPowerCalculator unitPowerCalculator,
    IUnitStatCalculators unitStatCalculators,
    IHeroSupportUnitProfileFactory supportUnitProfileFactory,
    IHeroProfileIdentifierFactory heroProfileIdentifierFactory) : IHohHeroProfileFactory
{
    public HeroProfile Create(HeroProfileIdentifier profileIdentifier, HeroDto hero, BuildingDto? barracks)
    {
        // if (barracks != null && profileIdentifier.BarracksLevel != barracks.Level)
        // {
        //     throw new ArgumentException("Barracks level does not match profile identifier");
        // }

        var adjustedStats = unitStatFactory.CreateHeroStats(hero, profileIdentifier.Level,
            profileIdentifier.AscensionLevel,
            profileIdentifier.AwakeningLevel, barracks);
        var power = unitPowerCalculator.CalculateHeroPower(adjustedStats, hero.StarClass,
            profileIdentifier.AbilityLevel,
            hero.Unit.Stats.ToDictionary(us => us.Type, us => us.Value));

        var supportUnit = supportUnitProfileFactory.Create(hero.BaseSupportUnit, barracks);

        return new HeroProfile
        {
            Identifier = profileIdentifier,
            Stats = adjustedStats,
            Power = power,
            SupportUnitProfile = supportUnit,
            AbilityChargeTime = unitStatCalculators.CalculateAbilityChargeTime(
                adjustedStats[UnitStatType.FocusRegen],
                adjustedStats[UnitStatType.MaxFocus]),
            AbilityInitialChargeTime = unitStatCalculators.CalculateAbilityInitialChargeTime(
                adjustedStats[UnitStatType.FocusRegen],
                adjustedStats[UnitStatType.Focus],
                adjustedStats[UnitStatType.MaxFocus]),
        };
    }

    public HeroProfile Create(BattleUnitDto battleUnit, HeroDto hero, BuildingDto? barracks)
    {
        var stats = unitStatFactory.CreateHeroStats(hero, battleUnit.Level, battleUnit.AscensionLevel,
            battleUnit.StatBoosts, barracks);

        var supportUnit = supportUnitProfileFactory.Create(hero.BaseSupportUnit, barracks);
        var statTotals = stats.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Sum(kvp2 => kvp2.Value));
        return new HeroProfile
        {
            Identifier = heroProfileIdentifierFactory.Create(hero.Id, battleUnit, barracks?.Level ?? 0),
            Stats = statTotals,
            StatsBreakdown = stats,
            Power = 0,
            SupportUnitProfile = supportUnit,
            AbilityChargeTime = unitStatCalculators.CalculateAbilityChargeTime(
                statTotals[UnitStatType.FocusRegen],
                statTotals[UnitStatType.MaxFocus]),
            AbilityInitialChargeTime = unitStatCalculators.CalculateAbilityInitialChargeTime(
                statTotals[UnitStatType.FocusRegen],
                statTotals[UnitStatType.Focus],
                statTotals[UnitStatType.MaxFocus]),
        };
    }
}
