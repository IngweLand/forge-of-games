using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class BattleEventBattleLocation : BattleLocationBase
{
    [ProtoMember(1)]
    public required string BattleEventBattleComponent { get; set; }

    [ProtoMember(2)]
    public required int EventId { get; set; }
}
