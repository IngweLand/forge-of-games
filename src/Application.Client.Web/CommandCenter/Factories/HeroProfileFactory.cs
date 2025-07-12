using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Factories;

public class HeroProfileFactory(
    IUnitStatFactory unitStatFactory,
    IUnitPowerCalculator unitPowerCalculator,
    IUnitStatCalculators unitStatCalculators,
    IHohHeroSupportUnitProfileFactory supportUnitProfileFactory) : IHohHeroProfileFactory
{
    public HeroProfile Create(BasicHeroProfile profileDto, HeroDto hero, BuildingDto? barracks)
    {
        var adjustedStats = unitStatFactory.CreateHeroStats(hero, profileDto.Level, profileDto.AscensionLevel,
            profileDto.AwakeningLevel, barracks);
        var power = unitPowerCalculator.CalculateHeroPower(adjustedStats, hero.StarClass, profileDto.AbilityLevel,
            hero.Unit.Stats.ToDictionary(us => us.Type, us => us.Value));

        var supportUnit = supportUnitProfileFactory.Create(hero.BaseSupportUnit, barracks);

        return new HeroProfile
        {
            Id = profileDto.Id,
            HeroId = profileDto.HeroId,
            Level = profileDto.Level,
            AscensionLevel = profileDto.AscensionLevel,
            AbilityLevel = profileDto.AbilityLevel,
            AwakeningLevel = profileDto.AwakeningLevel,
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
            BarracksLevel = barracks?.Level ?? 0,
        };
    }

    public HeroProfile Create(HeroProfile profile, HeroDto hero, BuildingDto? barracks)
    {
        var adjustedStats = unitStatFactory.CreateHeroStats(hero, profile.Level, profile.AscensionLevel,
            profile.AwakeningLevel, barracks);
        var power = unitPowerCalculator.CalculateHeroPower(adjustedStats, hero.StarClass, profile.AbilityLevel,
            hero.Unit.Stats.ToDictionary(us => us.Type, us => us.Value));

        var supportUnit = supportUnitProfileFactory.Create(hero.BaseSupportUnit, barracks);

        return new HeroProfile
        {
            Id = profile.Id,
            HeroId = profile.HeroId,
            Level = profile.Level,
            AscensionLevel = profile.AscensionLevel,
            AbilityLevel = profile.AbilityLevel,
            AwakeningLevel = profile.AwakeningLevel,
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
            BarracksLevel = barracks?.Level ?? 0,
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
            Id = hero.Id,
            HeroId = hero.Id,
            Level = battleUnit.Level,
            AscensionLevel = battleUnit.AscensionLevel,
            AbilityLevel = battleUnit.AbilityLevel,
            AwakeningLevel = 0,
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
            BarracksLevel = barracks?.Level ?? 0,
        };
    }
}
