using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class BattleUnit
{
    [ProtoMember(1)]
    public required BattleUnitProperties Properties { get; set; }

    [ProtoMember(2)]
    public BattleUnitState? UnitState { get; set; }
}

