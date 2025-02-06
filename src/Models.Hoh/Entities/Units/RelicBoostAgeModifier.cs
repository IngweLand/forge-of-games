using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class RelicBoostAgeModifier
{
    [ProtoMember(1)]
    public required IReadOnlyDictionary<string, float> AgeModifiers { get; init; } = new Dictionary<string, float>();

    [ProtoMember(2)]
    public required string Id { get; init; }
}
