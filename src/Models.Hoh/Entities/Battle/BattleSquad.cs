using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class BattleSquad
{
    [ProtoMember(1)]
    public int BattlefieldSlot { get; set; }

    [ProtoMember(2)]
    public BattleUnit? Hero { get; set; }

    [ProtoMember(3)]
    public BattleUnit? Unit { get; set; }
}

