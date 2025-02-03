using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Extensions;

public static class BuildingTypeExtensions
{
    public static string ToStringRepresentation(this BuildingType buildingType)
    {
        return buildingType switch
        {
            BuildingType.Barracks => "barracks",
            BuildingType.CityHall => "cityHall",
            BuildingType.Collectable => "collectable",
            BuildingType.CultureSite => "cultureSite",
            BuildingType.Evolving => "evolving",
            BuildingType.ExtractionPoint => "extractionPoint",
            BuildingType.Farm => "farm",
            BuildingType.GoldMine => "goldMine",
            BuildingType.Home => "home",
            BuildingType.Irrigation => "irrigation",
            BuildingType.PapyrusField => "papyrusField",
            BuildingType.RiceFarm => "riceFarm",
            BuildingType.Special => "special",
            BuildingType.Workshop => "workshop",
            BuildingType.Runestone => "runestone",
            BuildingType.Beehive => "beehive",
            BuildingType.FishingPier => "fishingPier",
            _ => string.Empty,
        };
    }
}
