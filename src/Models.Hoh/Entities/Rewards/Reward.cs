using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Rewards;

[ProtoContract]
public class Reward : RewardBase
{
    [ProtoMember(1)]
    public required string Id { get; set; }

    [ProtoMember(2)]
    public required IReadOnlyCollection<RewardBase> Rewards { get; init; }
}
