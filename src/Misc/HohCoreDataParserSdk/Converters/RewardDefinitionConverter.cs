using AutoMapper;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Rewards;

namespace Ingweland.Fog.HohCoreDataParserSdk.Converters;

public class RewardDefinitionConverter :ITypeConverter<RepeatedField<Any>, IReadOnlyCollection<RewardBase>>
{
    public IReadOnlyCollection<RewardBase> Convert(RepeatedField<Any> source, IReadOnlyCollection<RewardBase> destination, ResolutionContext context)
    {
        var list = new List<RewardBase>();
        var mapper = context.Mapper;
        foreach (var any in source)
        {
            if (any.Is(ResourceRewardDTO.Descriptor))
            {
                list.Add(mapper.Map<ResourceReward>(any.Unpack<ResourceRewardDTO>()));
            }
            else if (any.Is(MysteryChestRewardDTO.Descriptor))
            {
                list.Add(mapper.Map<MysteryChestReward>(any.Unpack<MysteryChestRewardDTO>()));
            }
            else if (any.Is(RewardDefinitionDTO.Descriptor))
            {
                list.Add(mapper.Map<Reward>(any.Unpack<RewardDefinitionDTO>()));
            }
            else if (any.Is(LootContainerRewardDTO.Descriptor))
            {
                list.Add(mapper.Map<LootContainerReward>(any.Unpack<LootContainerRewardDTO>()));
            }
            else if (any.Is(HeroRewardDto.Descriptor))
            {
                list.Add(mapper.Map<HeroReward>(any.Unpack<HeroRewardDto>()));
            }
            else if (any.Is(DynamicActionChangeRewardDTO.Descriptor))
            {
                list.Add(mapper.Map<DynamicActionChangeReward>(any.Unpack<DynamicActionChangeRewardDTO>()));
            }
            else if (any.Is(IncreaseExpansionRightRewardDTO.Descriptor))
            {
                list.Add(mapper.Map<IncreaseExpansionRightReward>(any.Unpack<IncreaseExpansionRightRewardDTO>()));
            }
        }

        return list.AsReadOnly();
    }
}
