using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class BattleSummary
{
    [ProtoMember(1)]
    public required string BattleDefinitionId { get; set; }

    [ProtoMember(2)]
    public required byte[] BattleId { get; set; }

    [ProtoMember(3)]
    public IReadOnlyCollection<BattleSquad> EnemySquads { get; set; } = new List<BattleSquad>();

    [ProtoMember(4)]
    public required BattleLocationBase Location { get; set; }

    [ProtoMember(5)]
    public int NextWaveIndex { get; set; }

    [ProtoMember(6)]
    public IReadOnlyCollection<BattleSquad> PlayerSquads { get; set; } = new List<BattleSquad>();

    [ProtoMember(7)]
    public BattleResultStatus ResultStatus { get; set; } = BattleResultStatus.Undefined;
}
