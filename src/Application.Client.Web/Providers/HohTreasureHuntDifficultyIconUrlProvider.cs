using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;

namespace Ingweland.Fog.Application.Client.Web.Providers;

public class HohTreasureHuntDifficultyIconUrlProvider(IAssetUrlProvider assetUrlProvider)
    : IHohTreasureHuntDifficultyIconUrlProvider
{
    public string GetIconUrl(int difficulty)
    {
        return assetUrlProvider.GetHohIconUrl($"icon_ath_difficulty_{difficulty}");
    }
}
