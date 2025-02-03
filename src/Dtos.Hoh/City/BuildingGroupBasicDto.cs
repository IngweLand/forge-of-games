using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.City;

[ProtoContract]
public class BuildingGroupBasicDto
{
    [ProtoMember(1)]
    public string? AssetId { get; init; }

    [ProtoMember(2)]
    public required BuildingGroup Id { get; init; }

    [ProtoMember(3)]
    public required string Name { get; init; }
}
