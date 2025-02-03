using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class UnitStatFormulaFactors
{
    [ProtoMember(1)]
    public float Normal { get; init; }
    [ProtoMember(2)]
    public float Ascension { get; init; }
}