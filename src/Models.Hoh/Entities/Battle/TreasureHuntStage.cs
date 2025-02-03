using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;
[ProtoContract]
public class TreasureHuntStage
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<BattleDetails> Battles { get; init; } = new List<BattleDetails>();
    [ProtoMember(2)]
    public required int Index { get; init; }
}