using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.HohCoreDataParserSdk.Extensions;

public static class ResourceTypeExtensions
{
    public static ResourceType ToResourceType(this string value)
    {
        return value.ToLower() switch
        {
            "alliance_research_point" => ResourceType.AllianceResearchPoint,
            "alliance_resource" => ResourceType.AllianceResource,
            "ascension_material" => ResourceType.AscensionMaterial,
            "blueprint" => ResourceType.Blueprint,
            "building_piece" => ResourceType.BuildingPiece,
            "ember" => ResourceType.Ember,
            "event_city_highscore" => ResourceType.EventCityHighscore,
            "evolution_token" => ResourceType.EvolutionToken,
            "gacha" => ResourceType.Gacha,
            "gacha_dust" => ResourceType.GachaDust,
            "good" => ResourceType.Good,
            "mastery_points" => ResourceType.MasteryPoints,
            "material" => ResourceType.Material,
            "mock" => ResourceType.Mock,
            "mock_woa" => ResourceType.MockWoa,
            "negotiation_wildcard" => ResourceType.NegotiationWildcard,
            "premium" => ResourceType.Premium,
            "pvp" => ResourceType.Pvp,
            "research_points" => ResourceType.ResearchPoints,
            "rift_token" => ResourceType.RiftToken,
            "soft_currency" => ResourceType.SoftCurrency,
            "treasure_hunt" => ResourceType.TreasureHunt,
            "upgrade_token" => ResourceType.UpgradeToken,
            "woa" => ResourceType.Woa,
            "wonder_contribution" => ResourceType.WonderContribution,
            "xp_scroll" => ResourceType.XpScroll,
            _ => throw new Exception($"Cannot map resource type: {value}"),
        };
    }
}
