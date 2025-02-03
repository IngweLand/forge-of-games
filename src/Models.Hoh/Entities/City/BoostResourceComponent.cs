using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class BoostResourceComponent : ComponentBase
{
    [ProtoMember(1)]
    public CityId CityId { get; init; }

    [ProtoMember(2)]
    public string? ResourceId { get; init; }
    [ProtoMember(3)]
    public ResourceType ResourceType { get; init; }

    [ProtoMember(4)]
    public double Value { get; init; }
}
