using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class MysteryChestRewardDTO
{
    public IList<ResourceRewardDTO> ResourceRewards => PackedRewards.FindAndUnpackToList<ResourceRewardDTO>();
    public IList<RewardDefinitionDTO> RewardDefinitions => PackedRewards.FindAndUnpackToList<RewardDefinitionDTO>();
}
