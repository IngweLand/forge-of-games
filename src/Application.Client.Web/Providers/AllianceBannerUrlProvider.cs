using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;

namespace Ingweland.Fog.Application.Client.Web.Providers;

public class AllianceBannerUrlProvider(IAssetUrlProvider assetUrlProvider) : IAllianceBannerUrlProvider
{
    public string GetIconUrl(int iconId, int colorId)
    {
        //TODO: Remove after we get icon/crest color ids
        return assetUrlProvider.GetHohIconUrl($"icon_alliance_symbol_0");
        return assetUrlProvider.GetHohIconUrl($"icon_alliance_symbol_{iconId}_{colorId}");
    }

    public string GetBackgroundUrl(int backgroundId, int colorId)
    {
        //TODO: Remove after we get icon/crest color ids
        return assetUrlProvider.GetHohIconUrl($"alliance_shield_0");
        return assetUrlProvider.GetHohIconUrl($"alliance_shield_{backgroundId}_{colorId}");
    }
}
