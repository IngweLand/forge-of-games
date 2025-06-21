using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Enums;

[ProtoContract]
public enum BattleResultStatus
{
    Undefined = 0,
    Win = 1,
    Defeat = 2,
}
