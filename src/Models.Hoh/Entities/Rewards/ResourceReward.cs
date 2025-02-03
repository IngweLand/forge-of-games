using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Rewards;

[ProtoContract]
public class ResourceReward : RewardBase
{
    [ProtoMember(1)]
    public int Amount { get; init; }

    [ProtoMember(2)]
    public required string ResourceId { get; init; }
}
