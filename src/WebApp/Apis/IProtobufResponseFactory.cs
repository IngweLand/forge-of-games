namespace Ingweland.Fog.WebApp.Apis;

public interface IProtobufResponseFactory
{
    ValueTask WriteToResponseAsync(HttpContext context, object payload);
    void WriteNotFoundToResponse(HttpContext context);
}
