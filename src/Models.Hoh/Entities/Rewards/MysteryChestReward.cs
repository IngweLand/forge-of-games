using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Rewards;

[ProtoContract]
public class MysteryChestReward : RewardBase
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<int> Probabilities { get; init; }

    [ProtoMember(2)]
    public required IReadOnlyCollection<RewardBase> Rewards { get; init; }
}
