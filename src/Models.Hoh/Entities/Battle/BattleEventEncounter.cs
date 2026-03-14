using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class BattleEventEncounter
{
    [ProtoMember(1)]
    public required BattleDetails BattleDetails { get; init; }

    [ProtoMember(2)]
    public required string Id { get; init; }

    [ProtoMember(3)]
    public int Index { get; set; }
}
