namespace Ingweland.Fog.Shared.Net;

public class HttpLoggingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var requestId = request.Headers.GetValues("X-Request-Id").ToString();
        var response = await base.SendAsync(request, cancellationToken);
        var rawPayload = await response.Content.ReadAsByteArrayAsync(cancellationToken);

        await SaveRawResponseToFileAsync(rawPayload, requestId ?? Guid.NewGuid().ToString());

        return response;
    }

    private async Task SaveRawResponseToFileAsync(byte[] payload, string requestId)
    {
        var filePath = $"D:\\Temp\\{requestId}.bin";
        await File.WriteAllBytesAsync(filePath, payload);
    }
}
