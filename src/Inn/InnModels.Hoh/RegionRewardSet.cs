using System.Collections;
using Google.Protobuf.Reflection;
using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class RegionRewardSet
{
    public IList<ResourceRewardDTO> ResourceRewards => PackedRewards.FindAndUnpackToList<ResourceRewardDTO>();

    public IncreaseExpansionRightRewardDTO? IncreaseExpansionRightReward =>
        PackedRewards.FindAndUnpackToList<IncreaseExpansionRightRewardDTO>().FirstOrDefault();
}
