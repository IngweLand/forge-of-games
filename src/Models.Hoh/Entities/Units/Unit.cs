using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class Unit : IUnit
{
    [ProtoMember(1)]
    public UnitColor Color { get; init; }

    [ProtoMember(2)]
    public required string Id { get; init; }

    [ProtoMember(3)]
    public required string Name { get; init; }

    [ProtoMember(4)]
    public required IReadOnlyCollection<UnitStat> Stats { get; init; }

    [ProtoMember(5)]
    public UnitType Type { get; init; }
    
    [ProtoMember(6)]
    public string? RarityId { get; init; }
}
