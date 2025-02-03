using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class BuildingBuffResource
{
    [ProtoMember(1)]
    public double Factor { get; init; }

    [ProtoMember(2)]
    public required string ResourceId { get; init; }
}
