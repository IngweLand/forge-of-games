using System.Globalization;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Application.Core.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class HeroSupportUnitViewModelFactory(
    IAssetUrlProvider assetUrlProvider,
    IUnitStatFactory unitStatFactory,
    IUnitPowerCalculator unitPowerCalculator)
    : IHeroSupportUnitViewModelFactory
{
    public HeroSupportUnitViewModel? Create(UnitDto baseSupportUnit, BuildingDto? barracks)
    {
        var component = barracks?.Components.OfType<BuildingUnitProviderComponent>().FirstOrDefault();
        IUnit unit;
        var unitLevel = 1;
        var assetId = baseSupportUnit.AssetId;
        if (component != null)
        {
            unit = component.BuildingUnit.Unit;
            unitLevel = component.BuildingUnit.Level;
            assetId = component.BuildingUnit.Unit.Name;
        }
        else
        {
            unit = baseSupportUnit;
        }

        var stats = unitStatFactory.CreateMainSupportUnitStats(unit, unitLevel, baseSupportUnit.StatCalculationFactors);
        var allStats = new Dictionary<UnitStatType, float>(stats);
        foreach (var us in unit.Stats.Where(us => !stats.ContainsKey(us.Type)))
        {
            allStats.Add(us.Type, us.Value);
        }

        var power = unitPowerCalculator.CalculateUnitPower(allStats);
        var statsItems = new List<IconLabelItemViewModel>()
        {
            new()
            {
                IconUrl = assetUrlProvider.GetHohIconUrl(
                    $"{unit.Type.GetTypeIconId()}_{unit.Color.ToString().ToLowerInvariant()}"),
                Label = unit.Stats.First(us => us.Type == UnitStatType.SquadSize).Value
                    .ToString(CultureInfo.InvariantCulture),
            },
        }.Concat(CreateStatsItems(stats)).ToList();
        return new HeroSupportUnitViewModel()
        {
            IconUrl = assetUrlProvider.GetHohUnitPortraitUrl(assetId),
            StatsItems = statsItems,
            Stats = allStats,
            Power = (int) Math.Ceiling(power),
        };
    }

    public HeroSupportUnitViewModel Create(HeroSupportUnitProfile profile)
    {
        
        var statsItems = new List<IconLabelItemViewModel>()
        {
            new()
            {
                IconUrl = assetUrlProvider.GetHohIconUrl(
                    $"{profile.Unit.Type.GetTypeIconId()}_{profile.Unit.Color.ToString().ToLowerInvariant()}"),
                Label = profile.Unit.Stats.First(us => us.Type == UnitStatType.SquadSize).Value
                    .ToString(CultureInfo.InvariantCulture),
            },
        }.Concat(CreateStatsItems(profile.Stats.Where(kvp =>
                kvp.Key is UnitStatType.Attack or UnitStatType.Defense or UnitStatType.MaxHitPoints or UnitStatType.BaseDamage)
            .ToDictionary())).ToList();
        return new HeroSupportUnitViewModel()
        {
            IconUrl = assetUrlProvider.GetHohUnitPortraitUrl(profile.AssetIt),
            StatsItems = statsItems,
            Power = (int) Math.Ceiling(profile.Power),
        };
    }

    private IList<IconLabelItemViewModel> CreateStatsItems(IReadOnlyDictionary<UnitStatType, float> stats)
    {
        return stats.Select(kvp => new IconLabelItemViewModel
        {
            IconUrl = assetUrlProvider.GetHohUnitStatIconUrl(kvp.Key),
            Label = kvp.Value.ToString(CultureInfo.InvariantCulture),
        }).ToList();
    }
}
