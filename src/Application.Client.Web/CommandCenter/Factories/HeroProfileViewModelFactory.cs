using System.Collections.ObjectModel;
using System.Globalization;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Factories;

public class HeroProfileViewModelFactory(
    IAssetUrlProvider assetUrlProvider,
    IHohHeroLevelSpecsProvider heroLevelSpecsProvider,
    IHeroSupportUnitViewModelFactory heroSupportUnitViewModelFactory,
    IHeroAbilityViewModelFactory abilityViewModelFactory,
    IBuildingLevelSpecsFactory buildingLevelSpecsFactory) : IHohHeroProfileViewModelFactory
{
    private const int DEFAULT_HITS_PER_MINUTE = 60;

    private static readonly List<UnitStatType> MainDisplayedStats =
    [
        UnitStatType.Attack,
        UnitStatType.Defense,
        UnitStatType.MaxHitPoints,
        UnitStatType.BaseDamage,
    ];

    private static readonly List<UnitStatType> DisplayedStats =
    [
        UnitStatType.Attack,
        UnitStatType.Defense,
        UnitStatType.MaxHitPoints,
        UnitStatType.BaseDamage,
        UnitStatType.AttackSpeed,
        UnitStatType.Evasion,
        UnitStatType.CritChance,
        UnitStatType.CritDamage,
        UnitStatType.SingleTargetDamageAmp,
        UnitStatType.AoeDamageAmp,
        UnitStatType.BasicAttackDamageAmp,
        UnitStatType.HealTakenAmp,
    ];

    private static readonly HashSet<UnitStatType> PercentageBasedStats =
    [
        UnitStatType.CritChance,
        UnitStatType.CritDamage,
        UnitStatType.SingleTargetDamageAmp,
        UnitStatType.HealTakenAmp,
        UnitStatType.Evasion,
    ];

    public HeroProfileViewModel Create(HeroProfile profile, HeroDto hero, IEnumerable<BuildingDto> barracks,
        HeroRelicViewModel? relic = null, bool withSupportUnit = true)
    {
        var maxLevel = Math.Max(hero.ProgressionCosts.Count, profile.Identifier.Level);
        var profileViewModel = new HeroProfileViewModel
        {
            Identifier = profile.Identifier,
            HeroUnitId = hero.Unit.Id,
            Name = hero.Unit.Name,
            PortraitUrl = assetUrlProvider.GetHohUnitPortraitUrl(hero.Unit.AssetId),
            StarCount = hero.StarClass.ToStarCount(),
            UnitColor = hero.Unit.Color.ToCssColor(),
            SupportUnit = withSupportUnit ? heroSupportUnitViewModelFactory.Create(profile.SupportUnitProfile) : null,
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
            HeroLevels = heroLevelSpecsProvider.Get(maxLevel, profile.Identifier.AscensionLevel),
            AbilityLevels = Enumerable.Range(1, hero.Ability.Levels.Count).ToList(),
            AwakeningLevels = Enumerable.Range(0, 6).ToList(),
            BarracksLevels = barracks.Select(buildingLevelSpecsFactory.Create).OrderBy(b => b.Level).ToList(),
            StatsItems = CreateMainStatsItems(profile.Stats),
            StatsBreakdown = CreateStatsBreakdownItems(profile.StatsBreakdown),
            VideoUrl = assetUrlProvider.GetHohUnitVideoUrl(profile.Identifier.HeroId),
            Ability = abilityViewModelFactory.Create(hero.Ability, profile.Identifier.AbilityLevel,
                profile.AbilityChargeTime, profile.AbilityInitialChargeTime),
            Relic = relic,
        };

        return profileViewModel;
    }

    private ReadOnlyCollection<IconLabelItemViewModel> CreateMainStatsItems(
        IReadOnlyDictionary<UnitStatType, float> stats)
    {
        var list = new List<IconLabelItemViewModel>();
        foreach (var stat in MainDisplayedStats)
        {
            if (!stats.TryGetValue(stat, out var value))
            {
                continue;
            }

            list.Add(new IconLabelItemViewModel
            {
                IconUrl = assetUrlProvider.GetHohUnitStatIconUrl(stat),
                Label = UnitStatToString(stat, value),
            });
        }

        return list.AsReadOnly();
    }

    private ReadOnlyCollection<UnitStatBreakdownViewModel> CreateStatsBreakdownItems(
        IReadOnlyDictionary<UnitStatType, IReadOnlyDictionary<UnitStatSource, float>> stats)
    {
        var list = new List<UnitStatBreakdownViewModel>();
        foreach (var stat in DisplayedStats)
        {
            if (!stats.TryGetValue(stat, out var breakdown) || breakdown.Count == 0)
            {
                continue;
            }

            list.Add(new UnitStatBreakdownViewModel
            {
                IconUrl = assetUrlProvider.GetHohUnitStatIconUrl(stat),
                Values = breakdown.ToDictionary(kvp => kvp.Key, kvp => UnitStatToString(stat, kvp.Value)),
                TotalValue = UnitStatToString(stat, breakdown.Values.Sum(v => v)),
            });
        }

        return list.AsReadOnly();
    }

    private static string UnitStatToString(UnitStatType stat, float value)
    {
        string label;
        if (stat == UnitStatType.AttackSpeed)
        {
            label = Math.Round(DEFAULT_HITS_PER_MINUTE * value, MidpointRounding.AwayFromZero)
                .ToString(CultureInfo.InvariantCulture);
        }
        else if (PercentageBasedStats.Contains(stat))
        {
            label = value.ToString("P1");
        }
        else
        {
            label = Math.Round(value, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);
        }

        return label;
    }
}
