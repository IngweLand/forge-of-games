using Ingweland.Fog.Shared.Helpers.Interfaces;

namespace Ingweland.Fog.WebApp.Apis;

public class ProtobufResponseFactory(IProtobufSerializer serializer) : IProtobufResponseFactory
{
    private const string CONTENT_TYPE = "application/x-protobuf";

    public ValueTask WriteToResponseAsync(HttpContext context, object payload)
    {
        var bytes = serializer.SerializeToBytes(payload);
        context.Response.ContentType = CONTENT_TYPE;
        context.Response.StatusCode = StatusCodes.Status200OK;
        return context.Response.Body.WriteAsync(bytes);
    }

    public void WriteNotFoundToResponse(HttpContext context)
    {
        context.Response.ContentType = CONTENT_TYPE;
        context.Response.StatusCode = StatusCodes.Status404NotFound;
    }
}
