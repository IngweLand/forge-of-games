using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Factories;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;

public class HeroDtoToViewModelConverter(
    IAssetUrlProvider assetUrlProvider,
    IUnitStatCalculators statCalculators,
    IHohHeroLevelSpecsProvider heroLevelSpecsProvider)
    : ITypeConverter<HeroDto, HeroViewModel>
{
    public HeroViewModel Convert(HeroDto source, HeroViewModel destination,
        ResolutionContext context)
    {
        var starCount = source.StarClass switch
        {
            HeroStarClass.Star_2 => 2,
            HeroStarClass.Star_3 => 3,
            HeroStarClass.Star_4 => 4,
            HeroStarClass.Star_5 => 5,
            _ => 0,
        };

        var levels = new List<HeroLevelViewModel>();
        var levelSpecs = heroLevelSpecsProvider.Get(source.ProgressionCosts.Count);
        foreach (var levelSpec in levelSpecs)
        {
            if (levelSpec.IsAscension)
            {
                var ascensionCost = source.AscensionCosts[levelSpec.AscensionLevel];
                levels.Add(new HeroLevelViewModel
                {
                    Title = $"{levelSpec.Level} > {levelSpec.Level + 10}",
                    Cost = context.Mapper.Map<IReadOnlyCollection<IconLabelItemViewModel>>(ascensionCost),
                    Stats = CreateLevelStats(source, levelSpec.Level, levelSpec.AscensionLevel),
                    LevelSpecs = levelSpec,
                });
            }
            else
            {
                var progressionCost = source.ProgressionCosts.Skip(levelSpec.Level - 1).Take(1).First();
                levels.Add(new HeroLevelViewModel
                {
                    Title = levelSpec.Level.ToString(),
                    Cost = levelSpec.Level < source.ProgressionCosts.Count
                        ? context.Mapper.Map<IReadOnlyCollection<IconLabelItemViewModel>>(progressionCost)
                        : null,
                    Stats = CreateLevelStats(source, levelSpec.Level, levelSpec.AscensionLevel),
                    LevelSpecs = levelSpec,
                });
            }
        }

        return new HeroViewModel
        {
            Name = source.Unit.Name,
            VideoUrl = assetUrlProvider.GetHohUnitVideoUrl(source.Id),
            PortraitUrl = assetUrlProvider.GetHohUnitPortraitUrl(source.Unit.AssetId),
            ImageUrl = assetUrlProvider.GetHohUnitImageUrl(source.Unit.AssetId),
            UnitTypeIconUrl =
                assetUrlProvider.GetHohIconUrl(
                    $"{source.Unit.Type.GetTypeIconId()}_{source.Unit.Color.ToString().ToLowerInvariant()}"),
            Id = source.Id,
            StarCount = starCount,
            UnitTypeName = source.Unit.TypeName,
            Levels = levels,
            Data = source,
        };
    }

    private IReadOnlyCollection<IconLabelItemViewModel> CreateLevelStats(HeroDto source, int level, int ascensionLevel)
    {
        return
        [
            CreateLevelStat(UnitStatType.Attack, source, level, ascensionLevel),
            CreateLevelStat(UnitStatType.Defense, source, level, ascensionLevel),
            CreateLevelStat(UnitStatType.MaxHitPoints, source, level, ascensionLevel),
            CreateLevelStat(UnitStatType.BaseDamage, source, level, ascensionLevel),
        ];
    }

    private IconLabelItemViewModel CreateLevelStat(UnitStatType unitStatType, HeroDto source, int level,
        int ascensionLevel)
    {
        return new IconLabelItemViewModel
        {
            IconUrl = assetUrlProvider.GetHohUnitStatIconUrl(unitStatType),
            Label = statCalculators.CalculateHeroStatValueForLevel(level, ascensionLevel,
                source.Unit.Stats.First(us => us.Type == unitStatType).Value,
                source.Unit.StatCalculationFactors[unitStatType]).ToString(),
        };
    }
}
