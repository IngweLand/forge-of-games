using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.City;

[ProtoContract]
public class BuildingCustomizationDto
{
    public static BuildingCustomizationDto None = new()
    {
        Id = "-",
        Components = new List<ComponentBase>(),
        Duration = 0,
        Name = "-",
        BuildingGroup = BuildingGroup.Undefined,
        CityId = CityId.Undefined,
    };

    [ProtoMember(1)]
    public required BuildingGroup BuildingGroup { get; init; }

    [ProtoMember(2)]
    public CityId CityId { get; init; }

    [ProtoMember(3)]
    public required IList<ComponentBase> Components { get; init; }

    [ProtoMember(4)]
    public required int Duration { get; init; }

    [ProtoMember(5)]
    public required string Id { get; init; }

    [ProtoMember(6)]
    public required string Name { get; init; }
}
