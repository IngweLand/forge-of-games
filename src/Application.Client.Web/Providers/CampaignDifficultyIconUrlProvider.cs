using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Providers;

public class CampaignDifficultyIconUrlProvider(IAssetUrlProvider assetUrlProvider) : ICampaignDifficultyIconUrlProvider
{
    public string GetIconUrl(Difficulty difficulty)
    {
        return difficulty switch
        {
            Difficulty.Hard => assetUrlProvider.GetHohIconUrl("icon_campaignmap_difficulty_2"),
            _ => assetUrlProvider.GetHohIconUrl("icon_campaignmap_difficulty_1")
        };
    }
}