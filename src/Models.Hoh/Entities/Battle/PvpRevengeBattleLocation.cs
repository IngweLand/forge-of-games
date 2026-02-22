using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Alliance;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class PvpRevengeBattleLocation : BattleLocationBase
{
    [ProtoMember(1)]
    public required string BattleDefinitionId { get; set; }

    [ProtoMember(2)]
    public int PvpEventId { get; set; }
}
