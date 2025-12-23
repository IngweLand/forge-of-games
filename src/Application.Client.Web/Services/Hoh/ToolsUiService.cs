using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Calculators.Interfaces;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Tools;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh;

public class ToolsUiService(
    IMapper mapper,
    ICityCalculators cityCalculators,
    IBuildingMultilevelCostViewModelFactory buildingMultilevelCostViewModelFactory,
    IHeroProgressionCalculators heroProgressionCalculators,
    ILogger<ToolsUiService> logger) : IToolsUiService
{
    public BuildingMultilevelCostViewModel CalculateBuildingMultiLevelCost(BuildingGroupViewModel buildingGroup,
        int currentLevel, int toLevel)
    {
        if (currentLevel > toLevel)
        {
            throw new ArgumentException("From level must be less than or equal to target level.", nameof(currentLevel));
        }

        var cost = cityCalculators.CalculateMultilevelCost(
            mapper.Map<IEnumerable<BuildingLevelCostData>>(buildingGroup.Buildings), currentLevel, toLevel);
        return buildingMultilevelCostViewModelFactory.Create(cost);
    }

    public IReadOnlyCollection<IconLabelItemViewModel> CalculateWonderLevelsCost(WonderViewModel wonder,
        int currentLevel, int targetLevel)
    {
        return CalculateWonderLevelsCost(wonder.Data, currentLevel, targetLevel);
    }
    
    public IReadOnlyCollection<IconLabelItemViewModel> CalculateWonderLevelsCost(WonderDto wonder,
        int currentLevel, int targetLevel)
    {
        if (currentLevel >= targetLevel)
        {
            logger.LogWarning("Current level must be less than target level.");
            return [];
        }
        return mapper.Map<IReadOnlyCollection<IconLabelItemViewModel>>(
            cityCalculators.CalculateWonderLevelsCost(wonder, currentLevel, targetLevel));
    }
    
    public IReadOnlyCollection<IconLabelItemViewModel> CalculateHeroProgressionCost(HeroDto hero,
        HeroLevelSpecs currentLevel, HeroLevelSpecs targetLevel)
    {
        return mapper.Map<IReadOnlyCollection<IconLabelItemViewModel>>(
            heroProgressionCalculators.CalculateProgressionCost(hero, currentLevel, targetLevel));
    }
}
