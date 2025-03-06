using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;

namespace Ingweland.Fog.Application.Client.Web.Providers;

public class AllianceAvatarUrlProvider(IAssetUrlProvider assetUrlProvider) : IAllianceAvatarUrlProvider
{
    public string GetIconUrl(int iconId)
    {
        return assetUrlProvider.GetHohIconUrl($"icon_alliance_symbol_{iconId}");
    }

    public string GetBackgroundUrl(int backgroundId)
    {
        return assetUrlProvider.GetHohIconUrl($"alliance_shield_{backgroundId}");
    }
}