using AutoMapper;
using HohProtoParser.Helpers;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Rewards;
using Ingweland.Fog.Models.Hoh.Enums;

namespace HohProtoParser.Converters;

public class RegionRewardDtoConverter :ITypeConverter<RegionRewardDto, RegionReward>
{
    public RegionReward Convert(RegionRewardDto source, RegionReward destination, ResolutionContext context)
    {
        return new RegionReward()
        {
            Rewards = context.Mapper.Map<IReadOnlyCollection<RewardBase>>(source.RewardSet.PackedRewards),
        };
    }
}
