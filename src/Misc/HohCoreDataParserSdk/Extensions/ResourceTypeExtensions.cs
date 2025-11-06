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
            "evolution_token" => ResourceType.EvolutionToken,
            "gacha_dust" => ResourceType.GachaDust,
            "good" => ResourceType.Good,
            "mastery_points" => ResourceType.MasteryPoints,
            "material" => ResourceType.Material,
            "mock" => ResourceType.Mock,
            "negotiation_wildcard" => ResourceType.NegotiationWildcard,
            "research_points" => ResourceType.ResearchPoints,
            "rift_token" => ResourceType.RiftToken,
            "soft_currency" => ResourceType.SoftCurrency,
            "treasure_hunt" => ResourceType.TreasureHunt,
            "xp_scroll" => ResourceType.XpScroll,
            "premium" => ResourceType.Premium,
            "pvp" => ResourceType.Pvp,
            "gacha" => ResourceType.Gacha,
            "wonder_contribution" => ResourceType.WonderContribution,
            "upgrade_token" => ResourceType.UpgradeToken,
            "ember" => ResourceType.Ember,
            _ => throw new Exception($"Cannot map resource type: {value}"),
        };
    }
}
