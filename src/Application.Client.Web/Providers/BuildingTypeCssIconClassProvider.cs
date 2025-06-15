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
            BuildingType.Beehive => "building-type-icon-beehive",
            BuildingType.CityHall => "",
            BuildingType.Collectable => "",
            BuildingType.CultureSite => "building-type-icon-culture-site",
            BuildingType.Evolving => "",
            BuildingType.ExtractionPoint => "",
            BuildingType.Farm => "building-type-icon-farm",
            BuildingType.FishingPier => "building-type-icon-fishing-pier",
            BuildingType.GoldMine => "building-type-icon-gold-mine",
            BuildingType.Home => "building-type-icon-home",
            BuildingType.Irrigation => "building-type-icon-irrigation",
            BuildingType.PapyrusField => "building-type-icon-papyrus-field",
            BuildingType.RiceFarm => "building-type-icon-rice-farm",
            BuildingType.Runestone => "",
            BuildingType.Special => "building-type-icon-special",
            BuildingType.Workshop => "building-type-icon-workshop",
            BuildingType.Aviary => "building-type-icon-aviary",
            BuildingType.Quarry => "building-type-icon-quarry",
            _ => default,
        };
    }
}
