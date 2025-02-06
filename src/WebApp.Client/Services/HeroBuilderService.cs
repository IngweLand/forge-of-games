using System.Globalization;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.WebApp.Client.Services.Abstractions;

namespace Ingweland.Fog.WebApp.Client.Services;

public class HeroBuilderService(
    IUnitService unitService,
    ICityService cityService,
    IHeroBuilderViewModelFactory heroBuilderViewModelFactory,
    IAssetUrlProvider assetUrlProvider,
    IUnitStatCalculators statCalculators,
    IHeroSupportUnitViewModelFactory heroSupportUnitViewModelFactory,
    IUnitPowerCalculator unitPowerCalculator) : IHeroBuilderService
{
    public async Task<HeroBuilderViewModel?> GetFormData(string heroId)
    {
        HeroDto? hero;
        try
        {
            hero = await unitService.GetHeroAsync(heroId);
            if (hero == null)
            {
                return null;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }

        BattleAbilityDto? heroAbility;
        try
        {
            heroAbility = await unitService.GetHeroAbilityAsync(heroId);
            if (heroAbility == null)
            {
                return null;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }

        IReadOnlyCollection<BuildingDto> barracks;
        try
        {
            barracks = await cityService.GetBarracks(hero.Unit.Type);
            if (barracks.Count == 0)
            {
                return null;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }

        return heroBuilderViewModelFactory.Create(hero, heroAbility, barracks);
    }

    public CustomHeroViewModel CreateCustomProfile(HeroBuilderViewModel data, HeroLevelSpecs level,
        int abilityLevel, int awakeningLevel, int barracksLevel)
    {
        BuildingDto? barracks = null;
        if (barracksLevel > 0)
        {
            barracks = data.Barracks.FirstOrDefault(b => b.Level == barracksLevel);
        }

        var abilityLevels = data.Ability.Levels.Take(abilityLevel).ToList();
        var abilityText = new HeroAbilityText(abilityLevels.Last(hal => hal.Description != null).Description!);

        var stats = new Dictionary<UnitStatType, float>()
        {
            {UnitStatType.Attack, CreateLevelStat(UnitStatType.Attack, data.Hero, level, awakeningLevel, barracks)},
            {UnitStatType.Defense, CreateLevelStat(UnitStatType.Defense, data.Hero, level, awakeningLevel, barracks)},
            {
                UnitStatType.MaxHitPoints,
                CreateLevelStat(UnitStatType.MaxHitPoints, data.Hero, level, awakeningLevel, barracks)
            },
            {
                UnitStatType.BaseDamage,
                CreateLevelStat(UnitStatType.BaseDamage, data.Hero, level, awakeningLevel, barracks)
            },
        };
        var allStats = new Dictionary<UnitStatType, float>(stats);
        foreach (var us in data.Hero.Unit.Stats.Where(us => !stats.ContainsKey(us.Type)))
        {
            allStats.Add(us.Type, us.Value);
        }

        var power = unitPowerCalculator.CalculateHeroPower(allStats, data.Hero.StarClass, abilityLevel,
            data.Hero.Unit.Stats.ToDictionary(us => us.Type, us => us.Value));
        return new CustomHeroViewModel()
        {
            PortraitUrl = assetUrlProvider.GetHohUnitPortraitUrl(data.Hero.Unit.AssetId),
            Stats = allStats,
            StatsItems = CreateStatsItems(stats).AsReadOnly(),
            AbilityDescription = abilityText.GetDescription(abilityLevels.Last().DescriptionItems),
            AbilityIconUrl = assetUrlProvider.GetHohHeroAbilityIconUrl(data.Ability.Id),
            SupportUnit = heroSupportUnitViewModelFactory.Create(data.Hero.BaseSupportUnit, barracks),
            Power = (int) Math.Ceiling(power),
        };
    }

    private float CreateLevelStat(UnitStatType unitStatType, HeroDto hero,
        HeroLevelSpecs level, int awakeningLevel, BuildingDto? barracks)
    {
        var barracksValue = 0f;
        var boostStat = barracks?.Components
            .OfType<HeroBuildingBoostComponent>()
            .FirstOrDefault()
            ?.UnitStats
            .FirstOrDefault(us => us.Type == unitStatType);
        if (boostStat != null)
        {
            barracksValue = boostStat.Value;
        }

        var awakeningLevels = hero.AwakeningComponent.Levels.Take(awakeningLevel)
            .Where(al => al.StatType == unitStatType).ToList();

        return statCalculators.CalculateHeroStatValueForLevel(level.Level, level.AscensionLevel,
            hero.Unit.Stats.First(us => us.Type == unitStatType).Value,
            hero.Unit.StatCalculationFactors[unitStatType], awakeningLevels, barracksValue);
    }

    private IList<IconLabelItemViewModel> CreateStatsItems(Dictionary<UnitStatType, float> stats)
    {
        return stats.Select(kvp => new IconLabelItemViewModel
        {
            IconUrl = assetUrlProvider.GetHohUnitStatIconUrl(kvp.Key),
            Label = kvp.Value.ToString(CultureInfo.InvariantCulture),
        }).ToList();
    }
}
