using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class AwakeningLevel
{
    [ProtoMember(1)]
    public bool IsPercentage { get; init; }

    [ProtoMember(2)]
    public required UnitStatType StatType { get; init; }

    [ProtoMember(3)]
    public required float Value { get; init; }
}
