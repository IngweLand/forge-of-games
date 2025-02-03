using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Enums;

[ProtoContract]
public enum HeroClassId
{
    Undefined = 0,
    AreaAttacker,
    Champion,
    Commander,
    Defender,
    Genius,
    GreatLeader,
    Healer,
    Manipulator,
    SingleStriker,
    Supporter,
}
