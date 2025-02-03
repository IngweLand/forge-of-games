using Ingweland.Fog.Models.Hoh.Entities.Rewards;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Abstractions;

[ProtoContract]
[ProtoInclude(100, typeof(DynamicActionChangeReward))]
[ProtoInclude(101, typeof(HeroReward))]
[ProtoInclude(102, typeof(IncreaseExpansionRightReward))]
[ProtoInclude(103, typeof(LootContainerReward))]
[ProtoInclude(104, typeof(MysteryChestReward))]
[ProtoInclude(105, typeof(ResourceReward))]
[ProtoInclude(106, typeof(Reward))]
public abstract class RewardBase
{
}
