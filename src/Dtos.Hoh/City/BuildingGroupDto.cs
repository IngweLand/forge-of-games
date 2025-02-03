using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.City;

[ProtoContract]
public class BuildingGroupDto
{
    [ProtoMember(1)]
    public string? AssetId { get; init; }

    [ProtoMember(2)]
    public required IReadOnlyCollection<BuildingDto> Buildings { get; init; } = new List<BuildingDto>();

    [ProtoMember(3)]
    public required string CityName { get; init; }

    [ProtoMember(4)]
    public required BuildingGroup Id { get; init; }

    [ProtoMember(5)]
    public required string Name { get; init; }

    [ProtoMember(6)]
    public required BuildingType Type { get; init; }

    [ProtoMember(7)]
    public required string TypeName { get; init; }
}
