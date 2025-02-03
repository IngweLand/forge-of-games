using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class Encounter
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<ResourceAmount> AutoVictoryCost { get; init; }

    [ProtoMember(2)]
    public required BattleDetails BattleDetails { get; init; }

    [ProtoMember(3)]
    public required IReadOnlyCollection<ResourceAmount> Cost { get; init; }

    [ProtoMember(4)]
    public Difficulty Difficulty { get; init; }

    [ProtoMember(5)]
    public required IReadOnlyCollection<RewardBase> FirstTimeCompletionBonus { get; init; }

    [ProtoMember(6)]
    public required string Id { get; init; }

    [ProtoMember(7)]
    public int Index { get; set; }

    [ProtoMember(8)]
    public required IReadOnlyCollection<RewardBase> Rewards { get; init; }
}
