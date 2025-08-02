using Ingweland.Fog.InnSdk.Hoh.Authentication.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.Shared.Extensions;

namespace Ingweland.Fog.InnSdk.Hoh.Net;

public class AuthenticationHandler(
    IGameConnectionManager connectionManager,
    IAuthenticationManager authenticationManager) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!request.Options.TryGetValue(new HttpRequestOptionsKey<GameWorldConfig>(nameof(GameWorldConfig)),
                out var world))
        {
            throw new InvalidOperationException($"{nameof(GameWorldConfig)} not found in request options.");
        }

        await authenticationManager.EnsureAuthenticatedAsync(world);
        var sessionData = connectionManager.Get(world.Id);
        if (sessionData == null)
        {
            throw new InvalidOperationException($"Session data is null for world {world.Id} after authentication.");
        }

        UpdateAuthHeaders(request, sessionData);

        return await base.SendAsync(request, cancellationToken);
    }

    private static void UpdateAuthHeaders(HttpRequestMessage request, GameConnectionSessionData sessionData)
    {
        request.SetHeader("X-AUTH-TOKEN", sessionData.SessionId);
        request.SetHeader("X-ClientVersion", sessionData.ClientVersion);
    }
}
