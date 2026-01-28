using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Application.Server.Errors;
using Ingweland.Fog.Application.Server.Services.Commands;
using Ingweland.Fog.Application.Server.Services.Queries;
using Ingweland.Fog.Dtos.Hoh;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Ingweland.Fog.WebApp.Apis;

public static class FogApi
{
    private const string PREFIX = "api";

    public static RouteGroupBuilder MapFogApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup(PREFIX);
        api.MapPost(FogUrlBuilder.ApiRoutes.CREATE_SHARE, CreateShareAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.GET_SHARED_RESOURCE_TEMPLATE, GetSharedResourceAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.GET_SHARED_CITY_STRATEGIES, GetSharedCityStrategiesAsync);
        return api;
    }

    private static async Task<Results<Created<CreatedShareDto>, BadRequest<IEnumerable<string>>, InternalServerError>>
        CreateShareAsync(
            [AsParameters] FogServices services,
            HttpContext context,
            SharedDataDto content,
            CancellationToken ct)
    {
        var command = new CreateShareCommand(content);
        var result = await services.Mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<DataTooLargeError>())
            {
                return TypedResults.BadRequest(result.Errors.Select(e => e.Message));
            }

            return TypedResults.InternalServerError();
        }

        var dto = result.Value;
        var location = $"https://{context.Request.Host}/{PREFIX}{
            FogUrlBuilder.ApiRoutes.GET_SHARED_RESOURCE_TEMPLATE.Replace("{shareId}", dto.Id)}";

        return TypedResults.Created(location, dto);
    }

    private static async Task<Results<Ok<SharedDataDto>, NotFound, InternalServerError>> GetSharedResourceAsync(
        string shareId,
        [AsParameters] StatsServices services,
        CancellationToken ct)
    {
        var query = new GetSharedResourceQuery(shareId);
        var result = await services.Mediator.Send(query, ct);

        if (result.IsFailed)
        {
            if (result.HasError<ResourceNotFoundError>())
            {
                return TypedResults.NotFound();
            }

            return TypedResults.InternalServerError();
        }

        return TypedResults.Ok(result.Value);
    }

    private static async Task<Ok<IReadOnlyCollection<CommunityCityStrategyDto>>> GetSharedCityStrategiesAsync(
        [AsParameters] StatsServices services,
        CancellationToken ct)
    {
        var query = new GetSharedCityStrategiesQuery();
        var result = await services.Mediator.Send(query, ct);
        result.LogIfFailed(nameof(FogApi));

        return TypedResults.Ok(result.IsSuccess ? result.Value : []);
    }
}
