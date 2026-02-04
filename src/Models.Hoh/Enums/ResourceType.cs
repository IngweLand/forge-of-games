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
    Ember,
    EventCityHighscore,
    EvolutionToken,
    Gacha,
    GachaDust,
    Good,
    MasteryPoints,
    Material,
    Mock,
    MockWoa,
    NegotiationWildcard,
    Premium,
    Pvp,
    ResearchPoints,
    RiftToken,
    SoftCurrency,
    TreasureHunt,
    UpgradeToken,
    Woa,
    WonderContribution,
    XpScroll,
}
