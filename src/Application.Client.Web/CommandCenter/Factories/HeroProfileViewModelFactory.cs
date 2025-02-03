using System.Globalization;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Factories;

public class HeroProfileViewModelFactory(
    IAssetUrlProvider assetUrlProvider,
    IHohHeroLevelSpecsProvider heroLevelSpecsProvider,
    IBuildingLevelRangesFactory buildingLevelRangesFactory,
    IHeroSupportUnitViewModelFactory heroSupportUnitViewModelFactory) : IHohHeroProfileViewModelFactory
{
    public HeroProfileViewModel CreateForCommandCenterProfile(HeroProfile profile, HeroDto hero)
    {
        return Create(profile, hero, null);
    }

    public HeroProfileViewModel CreateForPlayground(HeroProfile profile, HeroDto hero,
        IReadOnlyCollection<BuildingDto> barracks)
    {
        return Create(profile, hero, barracks);
    }

    private HeroProfileViewModel Create(HeroProfile profile, HeroDto hero,
        IReadOnlyCollection<BuildingDto>? barracks)
    {
        var levelSpecs = heroLevelSpecsProvider.Get(hero.ProgressionCosts.Count);
        var level = levelSpecs.First(ls => ls.Level == profile.Level && ls.AscensionLevel == profile.AscensionLevel);
        var group = hero.Unit.Type.ToBuildingGroup();
        IReadOnlyCollection<int>? barracksLevels = null;
        if (barracks != null)
        {
            var ranges = buildingLevelRangesFactory.Create(barracks);
            var barracksRanges = ranges[group];
            barracksLevels = Enumerable.Range(barracksRanges.StartLevel,
                barracksRanges.EndLevel - barracksRanges.StartLevel + 1).ToList();
        }

        var abilityLevels = hero.Ability.Levels.Take(profile.AbilityLevel).ToList();
        var abilityText = new HeroAbilityText(abilityLevels.Last(hal => hal.Description != null).Description!);
        var profileViewModel = new HeroProfileViewModel()
        {
            Id = profile.Id,
            HeroId = profile.HeroId,
            Name = hero.Unit.Name,
            Level = level,
            AbilityLevel = profile.AbilityLevel,
            AwakeningLevel = profile.AwakeningLevel,
            PortraitUrl = assetUrlProvider.GetHohUnitPortraitUrl(hero.Unit.AssetId),
            StarCount = hero.StarClass.ToStarCount(),
            UnitColor = hero.Unit.Color.ToCssColor(),
            SupportUnit = heroSupportUnitViewModelFactory.Create(profile.SupportUnitProfile),
            Power = (int) Math.Ceiling(profile.Power),
            UnitTypeName = hero.Unit.TypeName,
            UnitClassName = hero.ClassName,
            UnitClassIconUrl = assetUrlProvider.GetHohIconUrl(hero.ClassId.GetClassIconId()),
            UnitClassTintedIconUrl =
                assetUrlProvider.GetHohIconUrl($"{hero.ClassId.GetClassIconId()}_{
                    hero.Unit.Color.ToString().ToLowerInvariant()}"),
            UnitTypeIconUrl =
                assetUrlProvider.GetHohIconUrl(hero.Unit.Type.GetTypeIconId()),
            UnitTypeTintedIconUrl =
                assetUrlProvider.GetHohIconUrl($"{hero.Unit.Type.GetTypeIconId()}_{
                    hero.Unit.Color.ToString().ToLowerInvariant()}"),
            HeroLevels = heroLevelSpecsProvider.Get(hero.ProgressionCosts.Count),
            AbilityLevels = Enumerable.Range(1, hero.Ability.Levels.Count).ToList(),
            AwakeningLevels = Enumerable.Range(0, 6).ToList(),
            BarracksLevels = barracksLevels,
            BarracksLevel = profile.BarracksLevel,
            StatsItems = CreateStatsItems(profile.Stats.Where(kvp =>
                    kvp.Key is UnitStatType.Attack or UnitStatType.Defense or UnitStatType.MaxHitPoints
                        or UnitStatType.BaseDamage)
                .ToDictionary()),
            AbilityDescription = abilityText.GetDescription(abilityLevels.Last().DescriptionItems),
            AbilityIconUrl = assetUrlProvider.GetHohHeroAbilityIconUrl(hero.Ability.Id),
            AbilityChargeTime = $"{profile.AbilityChargeTime:F1}s",
            AbilityInitialChargeTime = $"{profile.AbilityInitialChargeTime:F1}s",
            AbilityInitialChargePercentage = MathF.Round(
                (profile.AbilityChargeTime - profile.AbilityInitialChargeTime) / profile.AbilityChargeTime * 100),
        };

        return profileViewModel;
    }

    private IReadOnlyCollection<IconLabelItemViewModel> CreateStatsItems(Dictionary<UnitStatType, float> stats)
    {
        return new List<IconLabelItemViewModel>(stats.Select(kvp => new IconLabelItemViewModel
        {
            IconUrl = assetUrlProvider.GetHohUnitStatIconUrl(kvp.Key),
            Label = kvp.Value.ToString(CultureInfo.InvariantCulture),
        }));
    }
}
