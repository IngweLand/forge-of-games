using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class BattleUnitState
{
    [ProtoMember(1)]
    public int InBattleId { get; set; }

    [ProtoMember(2)]
    public IReadOnlyDictionary<UnitStatType, float> UnitStats { get; set; } = new Dictionary<UnitStatType, float>();
}
