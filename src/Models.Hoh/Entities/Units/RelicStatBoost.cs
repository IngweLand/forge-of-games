using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class RelicStatBoost
{
    [ProtoMember(1)]
    public required string RelicBoostAgeModifierDefinitionId { get; init; }
    [ProtoMember(2)]
    public required StatBoost StatBoosts { get; init; }
}
