namespace Ingweland.Fog.WebApp.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static RouteHandlerBuilder MapProtobufGet(this IEndpointRouteBuilder endpoints, string pattern, Delegate handler)
    {
        return endpoints.MapGet(pattern, handler)
            .Produces(StatusCodes.Status200OK, contentType: "application/x-protobuf")
            .Produces(StatusCodes.Status404NotFound);
    }
    
    public static RouteHandlerBuilder MapProtobufPost<TRequest>(this IEndpointRouteBuilder endpoints, string pattern, Delegate handler) where TRequest : notnull
    {
        return endpoints.MapPost(pattern, handler)
            .Accepts<TRequest>("application/x-protobuf")
            .Produces(StatusCodes.Status200OK, contentType: "application/x-protobuf")
            .Produces(StatusCodes.Status404NotFound);
    }
}
