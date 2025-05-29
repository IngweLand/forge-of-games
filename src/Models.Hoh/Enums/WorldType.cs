using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Enums;

[ProtoContract]
public enum WorldType
{
    Undefined = 0,
    Campaign,
    Dungeon,
    TeslaStorm,
    HistoricBattle,
}
