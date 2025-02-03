using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class EncounterRewardDto
{
    public MysteryChestRewardDTO MysteryChestReward =>
        PackedRewards.FindAndUnpackToList<MysteryChestRewardDTO>().SingleOrDefault() ?? 
        PackedRewards.FindAndUnpackToList<RewardDefinitionDTO>().Single().MysteryChestReward;
    
    public IList<ResourceRewardDTO> ResourceRewards => PackedRewards.FindAndUnpackToList<ResourceRewardDTO>();
    public IncreaseExpansionRightRewardDTO? IncreaseExpansionRightReward =>
        PackedRewards.FindAndUnpackToList<IncreaseExpansionRightRewardDTO>().SingleOrDefault();
}
