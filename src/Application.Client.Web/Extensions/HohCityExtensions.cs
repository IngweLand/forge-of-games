using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Extensions;

public static class HohCityExtensions
{
    public static string GetBuildingTypeIconId(this BuildingType buildingType)
    {
        return buildingType switch
        {
            BuildingType.Barracks => "icon_flat_barracks",
            BuildingType.Home => "icon_flat_home",
            BuildingType.Workshop => "icon_flat_workshop",
            BuildingType.CultureSite => "icon_flat_cultureSite",
            BuildingType.Farm => "icon_flat_farm",
            BuildingType.Special => "icon_flat_special",
            _ => string.Empty,
        };
    }
}
