using System.Net.Http.Headers;
using Google.Protobuf;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Shared;
using Ingweland.Fog.Shared.Localization;

namespace Ingweland.Fog.HohProtoParser;

public class Downloader(
    IAuthenticationService authenticationService,
    HttpClient httpClient) : IDownloader
{
    private const string LOCALIZATION_URL = "https://zz1.heroesofhistorygame.com/game/loca";
    private const string GAMEDESIGN_URL = "https://zz1.heroesofhistorygame.com/game/gamedesign";
    private const string PROTOBUF_CONTENT_TYPE = "application/x-protobuf";
    private const string JSON_CONTENT_TYPE = "application/json";
    private const string DEFAULT_DIRECTORY = "downloads";
    private AuthResponse _authResponse = null!;
    private string _directory = null!;

    public async Task<DownloadResult> DownloadAsync(string? location)
    {
        _directory = location ?? DEFAULT_DIRECTORY;
        _authResponse = await authenticationService.Authenticate();
        Directory.CreateDirectory(_directory);
        await DownloadLocales();
        await DownloadJsonLocale();

        var gamedesignDescriptors = new List<(string ContentType, string Extension)>()
        {
            (PROTOBUF_CONTENT_TYPE, "bin"),
            (JSON_CONTENT_TYPE, "json"),
        };
        var result = new DownloadResult()
        {
            Directory = _directory,
        };
        foreach (var task in gamedesignDescriptors)
        {
            var filename = $"gamedesign_{DateTime.Today:dd.MM.yy}.{task.Extension}";
            await DownloadGamedesign(task.ContentType, filename);
            if (task.ContentType == PROTOBUF_CONTENT_TYPE)
            {
                result.GamedesignFileName = filename;
            }
        }

        return result;
    }

    private void AddRequiredHeaders(HttpRequestMessage request, string acceptContentType)
    {
        request.Headers.Add("X-AUTH-TOKEN", _authResponse.SessionId);
        request.Headers.Add("X-Request-Id", Guid.NewGuid().ToString());
        request.Headers.Add("X-Platform", "browser");
        request.Headers.Add("X-ClientVersion", _authResponse.ClientVersion);
        request.Headers.Add("Accept-Encoding", "gzip");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptContentType));
    }

    private async Task DownloadGamedesign(string acceptContentType, string filename)
    {
        var payload = new GamedesignRequest() {Checksum = "invalid"};
        using var request = new HttpRequestMessage(HttpMethod.Post, GAMEDESIGN_URL);
        request.Content = new ByteArrayContent(payload.ToByteArray());
        request.Content.Headers.ContentType = new MediaTypeHeaderValue(PROTOBUF_CONTENT_TYPE);
        AddRequiredHeaders(request, acceptContentType);
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var stream = await response.Content.ReadAsStreamAsync();
        stream.Position = 0;
        await using var fileStream = File.Create(Path.Combine(DEFAULT_DIRECTORY, filename));
        await response.Content.CopyToAsync(fileStream);
    }

    private async Task DownloadJsonLocale()
    {
        var payload = new LocalizationRequest()
        {
            Locale = "en_DK",
        };
        using var request = new HttpRequestMessage(HttpMethod.Post, LOCALIZATION_URL);
        request.Content = new ByteArrayContent(payload.ToByteArray());
        request.Content.Headers.ContentType = new MediaTypeHeaderValue(PROTOBUF_CONTENT_TYPE);
        AddRequiredHeaders(request, JSON_CONTENT_TYPE);
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var fileName = "loca_en-DK.json";
        var responseData = await response.Content.ReadAsStringAsync();
        await File.WriteAllTextAsync(Path.Combine(_directory, fileName), responseData);
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
            using var request = new HttpRequestMessage(HttpMethod.Post, LOCALIZATION_URL);
            request.Content = new ByteArrayContent(payload.ToByteArray());
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(PROTOBUF_CONTENT_TYPE);
            AddRequiredHeaders(request, PROTOBUF_CONTENT_TYPE);
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var fileName = $"loca_{kvp.Key}.bin";
            var stream = await response.Content.ReadAsStreamAsync();
            stream.Position = 0;
            await using var fileStream = File.Create(Path.Combine(_directory, fileName));
            await response.Content.CopyToAsync(fileStream);
        }
    }
}
