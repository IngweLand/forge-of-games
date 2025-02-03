using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Rewards;

[ProtoContract]
public class IncreaseExpansionRightReward : RewardBase
{
    [ProtoMember(1)]
    public int Amount { get; init; }

    [ProtoMember(2)]
    public CityId CityId { get; init; }
}
