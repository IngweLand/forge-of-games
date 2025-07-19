using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh;

[ProtoContract]
public class ResourceDto
{
    [ProtoMember(1)]
    public AgeDto? Age { get; init; }

    [ProtoMember(2)]
    public required IReadOnlySet<CityId> CityIds { get; init; } = new HashSet<CityId>();

    [ProtoMember(3)]
    public required string Id { get; init; }

    [ProtoMember(4)]
    public required string Name { get; init; }

    [ProtoMember(5)]
    public ResourceType Type { get; init; }
}
