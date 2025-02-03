using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
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
        var adjustedStats = unitStatFactory.CreateMainHeroStats(hero, profileDto.Level, profileDto.AscensionLevel,
            profileDto.AwakeningLevel, barracks);
        var power = unitPowerCalculator.CalculateHeroPower(adjustedStats, hero.StarClass, profileDto.AbilityLevel,
            hero.Unit.Stats.ToDictionary(us => us.Type, us => us.Value));

        var supportUnit = supportUnitProfileFactory.Create(hero.BaseSupportUnit, barracks);

        return new HeroProfile()
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
                focusRegeneration: adjustedStats[UnitStatType.FocusRegen],
                maxFocus: adjustedStats[UnitStatType.MaxFocus]),
            AbilityInitialChargeTime = unitStatCalculators.CalculateAbilityInitialChargeTime(
                focusRegeneration: adjustedStats[UnitStatType.FocusRegen],
                focus: adjustedStats[UnitStatType.Focus],
                maxFocus: adjustedStats[UnitStatType.MaxFocus]),
            BarracksLevel = barracks?.Level ?? 0,
        };
    }

    public HeroProfile Create(HeroProfile profile, HeroDto hero, BuildingDto? barracks)
    {
        var adjustedStats = unitStatFactory.CreateMainHeroStats(hero, profile.Level, profile.AscensionLevel,
            profile.AwakeningLevel, barracks);
        var power = unitPowerCalculator.CalculateHeroPower(adjustedStats, hero.StarClass, profile.AbilityLevel,
            hero.Unit.Stats.ToDictionary(us => us.Type, us => us.Value));

        var supportUnit = supportUnitProfileFactory.Create(hero.BaseSupportUnit, barracks);

        return new HeroProfile()
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
                focusRegeneration: adjustedStats[UnitStatType.FocusRegen],
                maxFocus: adjustedStats[UnitStatType.MaxFocus]),
            AbilityInitialChargeTime = unitStatCalculators.CalculateAbilityInitialChargeTime(
                focusRegeneration: adjustedStats[UnitStatType.FocusRegen],
                focus: adjustedStats[UnitStatType.Focus],
                maxFocus: adjustedStats[UnitStatType.MaxFocus]),
            BarracksLevel = barracks?.Level ?? 0,
        };
    }
}
