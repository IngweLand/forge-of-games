using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.CityPlanner.Stats;

public static class ProductionProviderHelper
{
    private static readonly HashSet<BuildingType> SelectableProductBuildingTypes =
    [
        BuildingType.Farm, BuildingType.Workshop, BuildingType.ExtractionPoint, BuildingType.Beehive,
        BuildingType.Quarry, BuildingType.FishingPier, BuildingType.GoldMine, BuildingType.PapyrusField,
        BuildingType.RiceFarm, BuildingType.CamelFarm,
    ];

    public static bool CanSelectProduct(BuildingType buildingType, BuildingGroup buildingGroup)
    {
        var canSelect = SelectableProductBuildingTypes.Contains(buildingType);

        if (!canSelect)
        {
            canSelect = buildingGroup switch
            {
                BuildingGroup.CollectableSchoolV2 => true,
                BuildingGroup.CollectableArchitectsStudioV2 => true,
                _ => false
            };
        }

        return canSelect;
    }
}