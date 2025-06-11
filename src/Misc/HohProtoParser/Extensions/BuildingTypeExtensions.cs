using Ingweland.Fog.Models.Hoh.Enums;

namespace HohProtoParser.Extensions;

public static class BuildingTypeExtensions
{
    public static BuildingType ToBuildingType(this string value)
    {
        return value switch
        {
            "barracks" => BuildingType.Barracks,
            "cityHall" => BuildingType.CityHall,
            "collectable" => BuildingType.Collectable,
            "cultureSite" => BuildingType.CultureSite,
            "evolving" => BuildingType.Evolving,
            "extractionPoint" => BuildingType.ExtractionPoint,
            "farm" => BuildingType.Farm,
            "goldMine" => BuildingType.GoldMine,
            "home" => BuildingType.Home,
            "irrigation" => BuildingType.Irrigation,
            "papyrusField" => BuildingType.PapyrusField,
            "riceFarm" => BuildingType.RiceFarm,
            "special" => BuildingType.Special,
            "workshop" => BuildingType.Workshop,
            "runestone" => BuildingType.Runestone,
            "beehive" => BuildingType.Beehive,
            "fishingPier" => BuildingType.FishingPier,
            "quarry" => BuildingType.Quarry,
            "aviary" => BuildingType.Aviary,
            "ritualSite" => BuildingType.RitualSite,
            _ => throw new Exception($"Cannot map building type: {value}"),
        };
    }
}
