using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;

namespace Ingweland.Fog.Application.Client.Web.Providers;

public class HohPlayerAvatarUrlProvider(IAssetUrlProvider assetUrlProvider) : IHohPlayerAvatarUrlProvider
{
    public string GetUrl(int avatarId)
    {
        return assetUrlProvider.GetHohPlayerAvatarUrl($"player_portrait_{avatarId}");
    }
}
