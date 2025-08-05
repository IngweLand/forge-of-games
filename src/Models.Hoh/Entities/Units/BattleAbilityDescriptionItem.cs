using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class BattleAbilityDescriptionItem
{
    [ProtoMember(1)]
    public required string Id { get; init; }

    [ProtoMember(2)]
    public required BattleAbilityDescriptionItemValue Value { get; init; }
}