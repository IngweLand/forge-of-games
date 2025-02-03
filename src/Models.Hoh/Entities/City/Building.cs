using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class Building
{
    [ProtoMember(1)]
    public Age? Age { get; init; }

    [ProtoMember(2)]
    public BuildingBuffDetails? BuffDetails { get; init; }

    [ProtoMember(3)]
    public CityId CityId { get; init; }

    [ProtoMember(4)]
    public required IList<ComponentBase> Components { get; init; }

    [ProtoMember(5)]
    public required string Id { get; init; }

    [ProtoMember(6)]
    public int Length { get; init; }

    [ProtoMember(7)]
    public int Level { get; init; }

    [ProtoMember(8)]
    public BuildingGroup Group { get; init; }

    [ProtoMember(9)]
    public BuildingType Type { get; init; }

    [ProtoMember(10)]
    public int Width { get; init; }

    [ProtoMember(11)]
    public string? AssetId { get; init; }
    
    [ProtoMember(12)]
    public ExpansionSubType ExpansionSubType { get; init; }
}
