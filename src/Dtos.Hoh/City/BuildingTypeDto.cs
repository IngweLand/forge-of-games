using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.City;

[ProtoContract]
public class BuildingTypeDto
{
    [ProtoMember(1)]
    public required CityId CityId { get; init; }

    [ProtoMember(2)]
    public required string CityName { get; init; }

    [ProtoMember(3)]
    public required IReadOnlyCollection<BuildingGroupBasicDto> Groups { get; init; } = new List<BuildingGroupBasicDto>();

    [ProtoMember(4)]
    public required BuildingType Id { get; init; }

    [ProtoMember(5)]
    public required string Name { get; init; }
}
