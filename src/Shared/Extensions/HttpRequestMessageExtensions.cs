namespace Ingweland.Fog.Shared.Extensions;

public static class HttpRequestMessageExtensions
{
    public static void SetHeader(this HttpRequestMessage request, string headerName, string headerValue)
    {
        request.Headers.Remove(headerName);
        request.Headers.Add(headerName, headerValue);
    }
}
