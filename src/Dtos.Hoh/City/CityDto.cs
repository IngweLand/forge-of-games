using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.City;

[ProtoContract]
public class CityDto
{
    [ProtoMember(1)]
    public required CityId Id { get; init; }
    [ProtoMember(2)]
    public required string Name { get; init; }
}
