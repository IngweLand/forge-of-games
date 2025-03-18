using System.Net.Http.Headers;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.InnSdk.Hoh.Constants;
using Ingweland.Fog.InnSdk.Hoh.Net.Abstractions;

namespace Ingweland.Fog.InnSdk.Hoh.Net;

public class GameApiClient(
    HttpClient httpClient,
    IGameConnectionManager connectionManager,
    IWebAuthenticationService authenticationService) : IGameApiClient
{
    private const string PROTOBUF_CONTENT_TYPE = "application/x-protobuf";
    private const string JSON_CONTENT_TYPE = "application/json";
    private readonly SemaphoreSlim _authenticationMutex = new(1, 1);

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

    private void AddRequiredHeaders(HttpRequestMessage request, string acceptContentType,
        GameConnectionSessionData sessionData)
    {
        request.Headers.Add("X-AUTH-TOKEN", sessionData.SessionId);
        request.Headers.Add("X-Request-Id", Guid.NewGuid().ToString());
        request.Headers.Add("X-Platform", "browser");
        request.Headers.Add("X-ClientVersion", sessionData.ClientVersion);
        request.Headers.Add("X-Action-At", DateTime.UtcNow.ToString("O"));
        request.Headers.Add("Accept-Encoding", "gzip");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptContentType));
    }

    private async Task<HttpContent> DoSendAsync(GameWorldConfig world, string url, byte[] payload,
        string acceptContentType)
    {
        await EnsureAuthenticated(world);
        var sessionData = connectionManager.Get(world.Id);
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = new ByteArrayContent(payload);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue(PROTOBUF_CONTENT_TYPE);
        AddRequiredHeaders(request, acceptContentType, sessionData!);
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return response.Content;
    }

    private async Task EnsureAuthenticated(GameWorldConfig world)
    {
        await _authenticationMutex.WaitAsync();
        if (connectionManager.Get(world.Id) != null)
        {
            _authenticationMutex.Release();
        }
        else
        {
            try
            {
                await authenticationService.Authenticate(world);
            }
            finally
            {
                _authenticationMutex.Release();
            }
        }
    }
}
