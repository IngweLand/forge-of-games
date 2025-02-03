using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.City;

[ProtoContract]
public class WonderBasicDto
{
    [ProtoMember(1)]
    public required CityId CityId { get; init; }
    [ProtoMember(2)]
    public required string CityName { get; init; }
    [ProtoMember(3)]
    public required WonderId Id { get; init; }
    [ProtoMember(4)]
    public required string WonderName { get; init; }
}
