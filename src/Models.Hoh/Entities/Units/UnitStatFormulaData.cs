using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class UnitStatFormulaData
{
    [ProtoMember(1)]
    public required UnitStatFormulaType Type { get; init; }
    [ProtoMember(2)]
    public required IReadOnlyDictionary<string, UnitStatFormulaFactors> RarityFactors { get; set; }
    [ProtoMember(3)]
    public float BaseFactor { get; set; }
}