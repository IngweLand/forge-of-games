using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class CityInitConfigs
{
    [ProtoMember(1)]
    public required InitialGridComponent Grid { get; init; }
    [ProtoMember(2)]
    public required string Id { get; init; }
}
