using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Units;

[ProtoContract]
public class UnitDto : IUnit
{
    [ProtoMember(1)]
    public required string AssetId { get; set; }

    [ProtoMember(2)]
    public UnitColor Color { get; init; }

    [ProtoMember(3)]
    public required string Id { get; init; }

    [ProtoMember(4)]
    public required string Name { get; init; }

    [ProtoMember(5)]
    public required IReadOnlyDictionary<UnitStatType, UnitStatFormulaFactors> StatCalculationFactors { get; init; }

    [ProtoMember(6)]
    public required IReadOnlyCollection<UnitStat> Stats { get; init; }

    [ProtoMember(7)]
    public UnitType Type { get; init; }

    [ProtoMember(8)]
    public required string TypeName { get; init; }
}