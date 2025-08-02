using Ingweland.Fog.InnSdk.Hoh.Authentication.Abstractions;

namespace Ingweland.Fog.InnSdk.Hoh.Net;

public class HttpContextDependencyHandler(IGameConnectionManager gameConnectionManager) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Options.Set(new HttpRequestOptionsKey<IGameConnectionManager>(nameof(IGameConnectionManager)),
            gameConnectionManager);
        return await base.SendAsync(request, cancellationToken);
    }
}
