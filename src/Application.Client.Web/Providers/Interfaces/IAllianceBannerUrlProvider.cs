namespace Ingweland.Fog.Application.Client.Web.Providers.Interfaces;

public interface IAllianceBannerUrlProvider
{
    string GetIconUrl(int iconId, int colorId);
    string GetBackgroundUrl(int backgroundId, int colorId);
}
