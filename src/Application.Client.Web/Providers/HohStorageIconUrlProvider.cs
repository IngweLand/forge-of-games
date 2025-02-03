using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Providers;

public class HohStorageIconUrlProvider : IHohStorageIconUrlProvider
{
    private readonly IAssetUrlProvider _assetUrlProvider;

    public HohStorageIconUrlProvider(IAssetUrlProvider assetUrlProvider)
    {
        _assetUrlProvider = assetUrlProvider;
    }

    public string GetIconUrl(string resourceId)
    {
        var id = resourceId switch
        {
            "coins" => "icon_gold_storage",
            "food" => "icon_food_storage",
            "good" => "icon_good_storage",
            _ => $"icon_storage",
        };

        return _assetUrlProvider.GetHohIconUrl(id);
    }
    
    public string GetIconUrl(BuildingType buildingType)
    {
        var id = buildingType switch
        {
            BuildingType.Home => "icon_gold_storage",
            BuildingType.Farm => "icon_food_storage",
            BuildingType.Workshop => "icon_good_storage",
            _ => $"icon_storage",
        };

        return _assetUrlProvider.GetHohIconUrl(id);
    }
}
