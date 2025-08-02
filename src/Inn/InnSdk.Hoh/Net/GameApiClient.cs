using System.Net.Http.Headers;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.InnSdk.Hoh.Constants;
using Ingweland.Fog.InnSdk.Hoh.Net.Abstractions;

namespace Ingweland.Fog.InnSdk.Hoh.Net;

public class GameApiClient(HttpClient httpClient) : IGameApiClient
{
    private const string PROTOBUF_CONTENT_TYPE = "application/x-protobuf";
    private const string JSON_CONTENT_TYPE = "application/json";

    public async Task<string> SendForJsonAsync(GameWorldConfig world, string path, byte[] payload)
    {
        var url = GameEndpoints.CreateUrl(world.Id, path);
        var responseContent = await DoSendAsync(world, url, payload, JSON_CONTENT_TYPE);
        return await responseContent.ReadAsStringAsync();
    }

    public async Task<byte[]> SendForProtobufAsync(GameWorldConfig world, string path, byte[] payload)
    {
        var url = GameEndpoints.CreateUrl(world.Id, path);
        var responseContent = await DoSendAsync(world, url, payload, PROTOBUF_CONTENT_TYPE);
        return await responseContent.ReadAsByteArrayAsync();
    }

    private async Task<HttpContent> DoSendAsync(GameWorldConfig world, string url, byte[] payload,
        string acceptContentType)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = new ByteArrayContent(payload);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue(PROTOBUF_CONTENT_TYPE);
        request.Options.Set(new HttpRequestOptionsKey<GameWorldConfig>(nameof(GameWorldConfig)), world);
        request.Options.Set(new HttpRequestOptionsKey<string>(HttpRequestOptionsKeys.ACCEPT_CONTENT_TYPE),
            acceptContentType);
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return response.Content;
    }
}
