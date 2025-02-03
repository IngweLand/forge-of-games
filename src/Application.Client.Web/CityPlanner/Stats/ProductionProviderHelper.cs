using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public static class ProductionProviderHelper
{
    public static bool CanSelectProduct(BuildingType buildingType, BuildingGroup buildingGroup)
    {
        var canSelect = buildingType switch
        {
            BuildingType.Farm => true,
            BuildingType.Workshop => true,
            _ => false,
        };

        if (!canSelect)
        {
            canSelect = buildingGroup switch
            {
                BuildingGroup.CollectableSchoolV2 => true,
                BuildingGroup.CollectableArchitectsStudioV2 => true,
                _ => false,
            };
        }

        return canSelect;
    }
}
