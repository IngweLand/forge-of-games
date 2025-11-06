using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Rewards;

namespace Ingweland.Fog.HohCoreDataParserSdk.Converters;

public class RegionRewardDtoConverter : ITypeConverter<RegionRewardDto, RegionReward>
{
    public RegionReward Convert(RegionRewardDto source, RegionReward destination, ResolutionContext context)
    {
        return new RegionReward()
        {
            Rewards = context.Mapper.Map<IReadOnlyCollection<RewardBase>>(source.RewardSet.PackedRewards),
        };
    }
}
