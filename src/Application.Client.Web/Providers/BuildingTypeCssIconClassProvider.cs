using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Providers;

public class BuildingTypeCssIconClassProvider : IBuildingTypeCssIconClassProvider
{
    public string? GetIcon(BuildingType buildingType)
    {
        var concreteIcon =
            GetConcreteIconCssClass(buildingType);
        return concreteIcon == null ? null : $"building-type-icon {GetConcreteIconCssClass(buildingType)}";
    }

    private string? GetConcreteIconCssClass(BuildingType buildingType)
    {
        return buildingType switch
        {
            BuildingType.Barracks => "building-type-icon-barracks",
            BuildingType.Beehive => "",
            BuildingType.CityHall => "",
            BuildingType.Collectable => "",
            BuildingType.CultureSite => "building-type-icon-culture-site",
            BuildingType.Evolving => "",
            BuildingType.ExtractionPoint => "",
            BuildingType.Farm => "building-type-icon-farm",
            BuildingType.FishingPier => "",
            BuildingType.GoldMine => "",
            BuildingType.Home => "building-type-icon-home",
            BuildingType.Irrigation => "",
            BuildingType.PapyrusField => "",
            BuildingType.RiceFarm => "",
            BuildingType.Runestone => "",
            BuildingType.Special => "building-type-icon-special",
            BuildingType.Workshop => "building-type-icon-workshop",
            _ => default,
        };
    }
}
