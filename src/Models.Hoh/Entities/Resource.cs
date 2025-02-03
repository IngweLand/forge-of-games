using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities;

[ProtoContract]
public class Resource
{
    [ProtoMember(1)]
    public Age? Age { get; init; }

    [ProtoMember(2)]
    public CityId CityId { get; init; }

    [ProtoMember(3)]
    public required string Id { get; init; }

    [ProtoMember(4)]
    public ResourceType Type { get; init; }
}
