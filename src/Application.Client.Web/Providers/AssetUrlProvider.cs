using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.Settings;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Options;

namespace Ingweland.Fog.Application.Client.Web.Providers;

public class AssetUrlProvider(IOptionsSnapshot<AssetsSettings> assetsSettings) : IAssetUrlProvider
{
    public string GetHohIconUrl(string assetId, string extension)
        => GetAssetUrl(assetsSettings.Value.HohIconsPath, $"{assetId}{extension}");

    public string GetHohIconUrl(string assetId)
        => GetAssetUrl(assetsSettings.Value.HohIconsPath, $"{assetId}.png");

    public string GetHohPlayerAvatarUrl(string avatarId)
        => GetAssetUrl(assetsSettings.Value.HohPlayerAvatarsPath, $"{avatarId}.png");

    public string GetHohTechnologyImageUrl(string assetId, string extension) =>
        GetAssetUrl(assetsSettings.Value.HohImagesBasePath, "technologies", $"{assetId}{extension}");

    public string GetHohTechnologyImageUrl(string assetId) =>
        GetAssetUrl(assetsSettings.Value.HohImagesBasePath, "technologies", $"{assetId}.png");

    public string GetHohImageUrl(string assetId, string extension)
        => GetAssetUrl(assetsSettings.Value.HohImagesBasePath, $"{assetId}{extension}");

    public string GetHohImageUrl(string assetId)
        => GetAssetUrl(assetsSettings.Value.HohImagesBasePath, $"{assetId}.png");

    public string GetHohUnitPortraitUrl(string assetId, string extension)
        => GetAssetUrl(assetsSettings.Value.HohUnitImagesPath, $"{assetId}{extension}");

    public string GetHohUnitPortraitUrl(string assetId)
        => GetAssetUrl(assetsSettings.Value.HohUnitImagesPath, $"{assetId}.png");

    public string GetHohUnitImageUrl(string assetId)
        => GetAssetUrl(assetsSettings.Value.HohUnitImagesPath, $"{assetId}_fullbody.png");

    public string GetHohUnitVideoUrl(string assetId, string extension)
        => GetAssetUrl(assetsSettings.Value.HohUnitVideosPath, $"{assetId}{extension}");

    public string GetHohUnitVideoUrl(string assetId)
        => GetAssetUrl(assetsSettings.Value.HohUnitVideosPath, $"{assetId}.mp4");

    public string GetHohUnitStatIconUrl(UnitStatType unitStatType) =>
        GetAssetUrl(assetsSettings.Value.HohIconsPath, $"icon_unit_stat_{unitStatType}.png");

    public string GetHohHeroAbilityIconUrl(string heroAbilityId)
    {
        var iconId = heroAbilityId.Replace('.', '_').ToLowerInvariant();
        return GetAssetUrl(assetsSettings.Value.HohIconsPath, $"icon_{iconId}.png");
    }

    public string GetHohWorkerIconUrl(CityId cityId)=>
        GetAssetUrl(assetsSettings.Value.HohIconsPath, $"icon_workers_city_{cityId.ToString().ToLowerInvariant()}.png");

    private string GetAssetUrl(params string[] pathElements)
    {
        var cleanElements = pathElements
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.Trim('/'));

        var basePath = assetsSettings.Value.BaseUrl.TrimEnd('/');

        var fullPath = string.Join("/",
            new[] {basePath, assetsSettings.Value.Version}
                .Concat(cleanElements));

        return fullPath;
    }
}
