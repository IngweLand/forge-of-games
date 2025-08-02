using System.Net.Http.Headers;
using Ingweland.Fog.InnSdk.Hoh.Constants;
using Ingweland.Fog.Shared.Extensions;

namespace Ingweland.Fog.InnSdk.Hoh.Net;

public class RequiredHeadersHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        request.Options.TryGetValue(new HttpRequestOptionsKey<string>(HttpRequestOptionsKeys.ACCEPT_CONTENT_TYPE),
            out var acceptContentType);
        
        request.SetHeader("X-Request-Id", Guid.NewGuid().ToString());
        request.SetHeader("X-Platform", "browser");
        request.SetHeader("Accept-Encoding", "gzip");
        request.SetHeader("X-Action-At", DateTime.UtcNow.ToString("O"));
        if (!string.IsNullOrWhiteSpace(acceptContentType))
        {
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptContentType));
        }
       
        
        return await base.SendAsync(request, cancellationToken);
    }
}
