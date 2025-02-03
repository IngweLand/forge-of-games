using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class HeroProgressionCostResource
{
    [ProtoMember(1)]
    public int Amount { get; init; }

    [ProtoMember(2)]
    public float ResourceFactor { get; init; }

    [ProtoMember(3)]
    public required string ResourceId { get; init; }
}
