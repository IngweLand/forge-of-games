using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Application.Core.Interfaces;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Shared.Helpers.Interfaces;

namespace Ingweland.Fog.Application.Client.Web.Providers;

public class WebClientHohGameLocalizationDataRepository(
    IHttpClientFactory httpClientFactory,
    IProtobufSerializer protobufSerializer) : IHohGameLocalizationDataRepository
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("DefaultProtobufClient");

    private LocalizationData? _data;

    public void Dispose()
    {
    }

    public IReadOnlyDictionary<string, IReadOnlyCollection<string>> GetEntries(string cultureCode)
    {
        

        return _data.Entries;
    }

    public async Task InitializeAsync()
    {
        if (_data == null)
        {
            var rawData = await DownloadBinaryAsync();
            _data = protobufSerializer.DeserializeFromBytes<LocalizationData>(rawData);
        }
    }

    private async Task<byte[]> DownloadBinaryAsync()
    {
        var response = await _httpClient.GetAsync($"{FogUrlBuilder.ApiRoutes.HOH_LOCALIZATION_DATA_PATH}/en-DK");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync();
    }
}
