using AutoMapper;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Rewards;

namespace HohProtoParser.Converters;

public class ResearchRewardDtoConverter : ITypeConverter<RepeatedField<Any>, IReadOnlyCollection<ResearchRewardBase>>
{
    public IReadOnlyCollection<ResearchRewardBase> Convert(RepeatedField<Any> source,
        IReadOnlyCollection<ResearchRewardBase> destination, ResolutionContext context)
    {
        var list = new List<ResearchRewardBase>();
        var mapper = context.Mapper;
        foreach (var any in source)
        {
            var rewardsToSkip = new List<MessageDescriptor>()
                {IncidentRewardDTO.Descriptor, UnlockQuestlineRewardDTO.Descriptor, ResourceRewardDTO.Descriptor};
            if (rewardsToSkip.Any(descriptor => any.Is(descriptor)))
            {
                continue;
            }

            if (any.Is(UnlockBuildingUpgradeRewardDTO.Descriptor))
            {
                list.Add(mapper.Map<UnlockBuildingUpgradeReward>(any.Unpack<UnlockBuildingUpgradeRewardDTO>()));
            }
            else if (any.Is(IncreaseBuildingLimitRewardDTO.Descriptor))
            {
                list.Add(mapper.Map<IncreaseBuildingLimitReward>(any.Unpack<IncreaseBuildingLimitRewardDTO>()));
            }
            else if (any.Is(InstantUpgradeRewardDTO.Descriptor))
            {
                list.Add(mapper.Map<InstantUpgradeReward>(any.Unpack<InstantUpgradeRewardDTO>()));
            }
            else if (any.Is(UnlockAgeRewardDTO.Descriptor))
            {
                list.Add(mapper.Map<UnlockAgeReward>(any.Unpack<UnlockAgeRewardDTO>()));
            }
            else if (any.Is(StorageCapRewardDTO.Descriptor))
            {
                list.Add(mapper.Map<StorageCapReward>(any.Unpack<StorageCapRewardDTO>()));
            }
            else if (any.Is(HeroTreasureHuntUnlockDifficultyRewardDTO.Descriptor))
            {
                list.Add(mapper.Map<HeroTreasureHuntUnlockDifficultyReward>(
                    any.Unpack<HeroTreasureHuntUnlockDifficultyRewardDTO>()));
            }
            else if (any.Is(UnlockGoodRewardDTO.Descriptor))
            {
                list.Add(mapper.Map<UnlockGoodReward>(any.Unpack<UnlockGoodRewardDTO>()));
            }
            else if (any.Is(UnlockFeatureRewardDTO.Descriptor))
            {
                list.Add(mapper.Map<UnlockFeatureReward>(any.Unpack<UnlockFeatureRewardDTO>()));
            }
            else if (any.Is(UnlockBuildingRewardDTO.Descriptor))
            {
                list.Add(mapper.Map<UnlockBuildingReward>(any.Unpack<UnlockBuildingRewardDTO>()));
            }
            else
            {
                throw new Exception($"Unknown research reward type: {any.TypeUrl}");
            }
        }

        return list.AsReadOnly();
    }
}
