using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class HeroBattleAbilityComponentLevel
{
    [ProtoMember(1)]
    public required string AbilityId { get; init; }

    [ProtoMember(2)]
    public int Cost { get; init; }

    [ProtoMember(3)]
    public bool IsKeyLevel { get; init; }

    [ProtoMember(4)]
    public required IReadOnlyCollection<string> AbilityIds { get; init; }
}
