using Ingweland.Fog.Models.Hoh.Entities.Rewards;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Abstractions;

[ProtoContract]
[ProtoInclude(100, typeof(InstantUpgradeReward))]
[ProtoInclude(101, typeof(StorageCapReward))]
[ProtoInclude(102, typeof(UnlockAgeReward))]
[ProtoInclude(103, typeof(UnlockBuildingReward))]
[ProtoInclude(104, typeof(UnlockBuildingUpgradeReward))]
[ProtoInclude(105, typeof(UnlockFeatureReward))]
[ProtoInclude(106, typeof(UnlockGoodReward))]
[ProtoInclude(107, typeof(HeroTreasureHuntUnlockDifficultyReward))]
[ProtoInclude(108, typeof(IncreaseBuildingLimitReward))]
public abstract class ResearchRewardBase
{
}
