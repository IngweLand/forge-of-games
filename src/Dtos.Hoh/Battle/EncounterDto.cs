using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

[ProtoContract]
public class EncounterDto
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<RewardBase> FirstTimeCompletionBonus { get; init; }

    [ProtoMember(2)]
    public required string Id { get; init; }

    [ProtoMember(3)]
    public required IReadOnlyCollection<RewardBase> Rewards { get; init; }

    [ProtoMember(4)]
    public required IReadOnlyCollection<BattleWave> Waves { get; init; }

    [ProtoMember(5)]
    public int AvailableHeroSlots { get; init; } = 5;
}