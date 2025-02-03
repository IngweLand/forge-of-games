using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Tools;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;

public interface IToolsUiService
{
    BuildingMultilevelCostViewModel CalculateBuildingMultiLevelCost(BuildingGroupViewModel buildingGroup,
        int currentLevel, int toLevel);

    IReadOnlyCollection<IconLabelItemViewModel> CalculateWonderLevelsCost(WonderViewModel wonder, int currentLevel,
        int targetLevel);

    IReadOnlyCollection<IconLabelItemViewModel> CalculateHeroProgressionCost(HeroDto hero,
        HeroLevelSpecs currentLevel, HeroLevelSpecs targetLevel);
}
