using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Research;

[ProtoContract]
public class TechnologyDto
{
    [ProtoMember(1)]
    public required AgeDto Age { get; init; }

    [ProtoMember(2)]
    public CityId CityId { get; set; }

    [ProtoMember(3)]
    public required IReadOnlyCollection<ResourceAmount> Costs { get; init; }

    [ProtoMember(4)]
    public int HorizontalIndex { get; set; }

    [ProtoMember(5)]
    public required string Id { get; init; }

    [ProtoMember(6)]
    public required string Name { get; init; }

    [ProtoMember(7)]
    public required IReadOnlyCollection<string> ParentTechnologies { get; init; } = new List<string>();

    [ProtoMember(8)]
    public int VerticalIndex { get; set; }
}
