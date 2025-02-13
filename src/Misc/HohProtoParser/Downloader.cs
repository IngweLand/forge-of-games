using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Authentication;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.Shared.Localization;

namespace Ingweland.Fog.HohProtoParser;

public class Downloader(IInnSdkClient sdkClient) : IDownloader
{
    private const string DEFAULT_DIRECTORY = "downloads";
    private string _directory = null!;
    private GameWorldConfig _betaWorldConfig = new("zz", 1, "beta");

    public async Task<DownloadResult> DownloadAsync(string? location)
    {
        _directory = location ?? DEFAULT_DIRECTORY;
        Directory.CreateDirectory(_directory);
        await DownloadLocales();
        await DownloadJsonLocale();

        var result = new DownloadResult()
        {
            Directory = _directory,
        };
        var filename = $"gamedesign_{DateTime.Today:dd.MM.yy}.bin";
        await DownloadProtobufGamedesign(filename);
        result.GamedesignFileName = filename;
        filename = $"gamedesign_{DateTime.Today:dd.MM.yy}.json";
        await DownloadJsonGamedesign(filename);

        return result;
    }

    private async Task DownloadJsonGamedesign(string filename)
    {
        await File.WriteAllTextAsync(Path.Combine(DEFAULT_DIRECTORY, filename),
            await sdkClient.StaticDataService.GetGameDesignJsonAsync(_betaWorldConfig));
    }

    private async Task DownloadJsonLocale()
    {
        var filename = "loca_en-DK.json";
        var response = await sdkClient.StaticDataService.GetLocalizationJsonAsync(_betaWorldConfig, "en_DK");
        await File.WriteAllTextAsync(Path.Combine(_directory, filename), response);
    }

    private async Task DownloadLocales()
    {
        var locales = HohSupportedCultures.AllCultures.ToDictionary(c => c, c => c.Replace('-', '_'));
        foreach (var kvp in locales)
        {
            var payload = new LocalizationRequest()
            {
                Locale = kvp.Value,
            };
            var filename = $"loca_{kvp.Key}.bin";
            var response = await sdkClient.StaticDataService.GetLocalizationProtobufAsync(_betaWorldConfig, kvp.Value);
            await File.WriteAllBytesAsync(Path.Combine(_directory, filename), response);
        }
    }

    private async Task DownloadProtobufGamedesign(string filename)
    {
        await File.WriteAllBytesAsync(Path.Combine(DEFAULT_DIRECTORY, filename),
            await sdkClient.StaticDataService.GetGameDesignProtobufAsync(_betaWorldConfig));
    }
}
