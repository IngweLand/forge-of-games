using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Application.Core.Interfaces;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Shared.Helpers.Interfaces;

namespace Ingweland.Fog.Application.Client.Web.Providers;

public class WebClientHohDataProvider(IHttpClientFactory httpClientFactory, IProtobufSerializer protobufSerializer)
    : IHohDataProvider
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("DefaultProtobufClient");

    private Data? _data;

    public async Task<Data> GetDataAsync()
    {
        if (_data == null)
        {
            var rawData = await DownloadBinaryAsync();
            _data = protobufSerializer.DeserializeFromBytes<Data>(rawData);
        }

        return _data;
    }

    private async Task<byte[]> DownloadBinaryAsync()
    {
        var response = await _httpClient.GetAsync(FogUrlBuilder.ApiRoutes.HOH_CORE_DATA_PATH);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync();
    }
}
