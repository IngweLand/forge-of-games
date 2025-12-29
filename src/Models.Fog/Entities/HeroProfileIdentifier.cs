using ProtoBuf;

namespace Ingweland.Fog.Models.Fog.Entities;

[ProtoContract]
public record HeroProfileIdentifier
{
    [ProtoMember(4)]
    public int AbilityLevel { get; init; } = 1;

    [ProtoMember(3)]
    public int AscensionLevel { get; init; }

    [ProtoMember(5)]
    public int AwakeningLevel { get; init; }

    [ProtoMember(6)]
    public int BarracksLevel { get; init; }

    [ProtoMember(1)]
    public required string HeroId { get; init; }

    [Obsolete]
    public string? Id { get; init; }

    [ProtoMember(2)]
    public int Level { get; init; } = 1;
}
