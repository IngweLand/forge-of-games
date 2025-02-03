using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Rewards;

[ProtoContract]
public class RegionReward
{
    [ProtoMember(1)]
    public Difficulty Difficulty { get; init; }

    [ProtoMember(2)]
    public required IReadOnlyCollection<RewardBase> Rewards { get; init; }
}
