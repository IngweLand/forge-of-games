using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Enums;

[ProtoContract]
public enum ContinentId
{
    Undefined = 0,
    DesertDelta,
    EasternValley,
    FrozenFjord,
    Panganea,
    TeslaStormBlue,
    TeslaStormGreen,
    TeslaStormPurple,
    TeslaStormRed,
    TeslaStormYellow,
    VolcanicJungle,
}
