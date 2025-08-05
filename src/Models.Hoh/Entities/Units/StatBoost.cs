using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class StatBoost
{
    [ProtoMember(1)]
    public Calculation Calculation { get; set; }
    [ProtoMember(2)]
    public int Order { get; set; }
    [ProtoMember(3)]
    public StatAttribute? StatAttribute { get; set; }
    [ProtoMember(4)]
    public UnitStatType UnitStatType { get; set; }
    [ProtoMember(5)]
    public float Value { get; set; }
}
