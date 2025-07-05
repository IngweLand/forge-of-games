using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Enums;

[ProtoContract]
public enum Difficulty
{
    Undefined = 0,
    Normal = 1,
    Hard = 2,
}
