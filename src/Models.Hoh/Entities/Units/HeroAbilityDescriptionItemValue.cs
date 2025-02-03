using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class HeroAbilityDescriptionItemValue
{
    [ProtoMember(1)]
    public required NumericValueType Type { get; init; }
    [ProtoMember(2)]
    public required double Value { get; init; }
}