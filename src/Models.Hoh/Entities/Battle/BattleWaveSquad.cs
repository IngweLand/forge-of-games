using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class BattleWaveSquad
{
    [ProtoMember(1)]
    public int BattlefieldSlot { get; set; }

    [ProtoMember(2)]
    public BattleUnitProperties? Hero { get; set; }

    [ProtoMember(3)]
    public BattleUnitProperties? SupportUnit { get; set; }

    public BattleUnitProperties Unit => Hero ?? SupportUnit!;
}
