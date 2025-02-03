using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class RewardDefinitionDTO
{
    public MysteryChestRewardDTO MysteryChestReward => PackedRewards.FindAndUnpackToList<MysteryChestRewardDTO>().Single();
    public LootContainerRewardDTO LootContainerReward => PackedRewards.FindAndUnpackToList<LootContainerRewardDTO>().Single();
    public IList<HeroRewardDto> HeroRewards => PackedRewards.FindAndUnpackToList<HeroRewardDto>();
}
