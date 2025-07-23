using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Shared.Helpers;
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
        var concreteId = HohStringParser.GetConcreteId(resourceId);
        var id = concreteId switch
        {
            "good" => "icon_good_storage",
            _ => $"icon_{concreteId}",
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
