using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Enums;

[ProtoContract]
public enum BuildingType
{
    Undefined = 0,
    Barracks,
    Beehive,
    CityHall,
    Collectable,
    CultureSite,
    Evolving,
    ExtractionPoint,
    Farm,
    FishingPier,
    GoldMine,
    Home,
    Irrigation,
    PapyrusField,
    RiceFarm,
    Runestone,
    Special,
    Workshop,
}
