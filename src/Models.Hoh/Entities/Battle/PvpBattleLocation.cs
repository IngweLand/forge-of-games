using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Alliance;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class PvpBattleLocation : BattleLocationBase
{
    [ProtoMember(1)]
    public required string BattleDefinitionId { get; set; }

    [ProtoMember(2)]
    public required HohPlayer Enemy { get; set; }

    [ProtoMember(3)]
    public HohAlliance? EnemyAlliance { get; set; }

    [ProtoMember(4)]
    public int EnemyRankingPoints { get; set; }

    [ProtoMember(5)]
    public required PvpResultPoints PointsOnLoss { get; set; }

    [ProtoMember(6)]
    public required PvpResultPoints PointsOnWin { get; set; }

    [ProtoMember(7)]
    public int PvpEventId { get; set; }
}
