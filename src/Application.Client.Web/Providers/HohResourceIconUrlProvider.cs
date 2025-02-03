using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using ResourceType = Ingweland.Fog.Models.Hoh.Enums.ResourceType;

namespace Ingweland.Fog.Application.Client.Web.Providers;

public class HohResourceIconUrlProvider : IHohResourceIconUrlProvider
{
    private readonly IAssetUrlProvider _assetUrlProvider;

    public HohResourceIconUrlProvider(IAssetUrlProvider assetUrlProvider)
    {
        _assetUrlProvider = assetUrlProvider;
    }

    public string GetIconUrl(string resourceId)
    {
        var id = resourceId switch
        {
            "resource.hero_xp" => "icon_hero_xp",
            "resource.mastery_points" => "icon_mastery_points",
            "good" => "icon_good_storage",
            _ => $"icon_{resourceId}",
        };

        return _assetUrlProvider.GetHohIconUrl(id);
    }

    public string GetIconUrl(ResourceType resourceType)
    {
        var id = resourceType switch
        {
            ResourceType.Good => "icon_good_storage",
            _ => $"icon_{resourceType}",
        };

        return _assetUrlProvider.GetHohIconUrl(id);
    }
}
