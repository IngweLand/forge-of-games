using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class UnitStat
{
    [ProtoMember(1)]
    public UnitStatType Type { get; set; }

    [ProtoMember(2)]
    public float Value { get; set; }
}
