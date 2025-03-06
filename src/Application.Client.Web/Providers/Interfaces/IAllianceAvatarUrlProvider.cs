namespace Ingweland.Fog.Application.Client.Web.Providers.Interfaces;

public interface IAllianceAvatarUrlProvider
{
    string GetIconUrl(int iconId);
    string GetBackgroundUrl(int backgroundId);
}