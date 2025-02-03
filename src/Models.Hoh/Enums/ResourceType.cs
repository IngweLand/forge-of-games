using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Enums;

[ProtoContract]
public enum ResourceType
{
    Undefined = 0,
    AllianceResearchPoint,
    AllianceResource,
    AscensionMaterial,
    Blueprint,
    BuildingPiece,
    EvolutionToken,
    Gacha,
    GachaDust,
    Good,
    MasteryPoints,
    Material,
    Mock,
    NegotiationWildcard,
    Premium,
    Pvp,
    ResearchPoints,
    RiftToken,
    SoftCurrency,
    TreasureHunt,
    UpgradeToken,
    WonderContribution,
    XpScroll,
}
