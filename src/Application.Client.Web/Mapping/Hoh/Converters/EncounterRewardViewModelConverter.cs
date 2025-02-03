using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Rewards;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;

public class EncounterRewardViewModelConverter(IHohResourceIconUrlProvider resourceIconUrlProvider)
    : ITypeConverter<RewardBase, IList<EncounterRewardViewModel>>
{
    public IList<EncounterRewardViewModel> Convert(RewardBase source, IList<EncounterRewardViewModel> destination,
        ResolutionContext context)
    {
        if (source is MysteryChestReward mcr)
        {
            return mcr.Rewards
                .Zip(mcr.Probabilities, (reward, probability) =>
                {
                    var resourceReward = (ResourceReward) reward;
                    return new EncounterRewardViewModel
                    {
                        Title = resourceReward.Amount.ToString(),
                        Subtitle = $"{probability}%",
                        IconUrl = resourceIconUrlProvider.GetIconUrl(resourceReward.ResourceId),
                    };
                })
                .ToList();
        }

        throw new Exception($"{source} conversion is not supported");
    }
}
