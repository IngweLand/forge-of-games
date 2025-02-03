using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class UnitBattleConstants
{
    [ProtoMember(1)]
    public required IReadOnlyDictionary<UnitStatFormulaType,UnitStatType> FormulaTypeToStatTypeMap { get; init; }

    [ProtoMember(2)]
    public required IReadOnlyCollection<UnitStat> BaseValues { get; init; }
}