using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities;

[ProtoContract]
public class ResourceAmount
{
    [ProtoMember(1)]
    public int Amount { get; init; }

    [ProtoMember(2)]
    public required string ResourceId { get; init; }
}
