using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class LootContainerRewardDTO
{
    public IList<ResourceRewardDTO> Rewards => PackedRewards.FindAndUnpackToList<ResourceRewardDTO>();
}
