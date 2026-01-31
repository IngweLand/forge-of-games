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
        api.MapGet(FogUrlBuilder.ApiRoutes.GET_COMMUNITY_CITY_STRATEGIES, GetCommunityCityStrategiesAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.GET_COMMUNITY_CITY_GUIDES, GetCommunityCityGuidesAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.GET_COMMUNITY_CITY_GUIDE_TEMPLATE, GetCommunityCityGuideAsync);
        api.MapPost(FogUrlBuilder.ApiRoutes.UPLOAD_SHARED_IMAGE, UploadSharedImageAsync);
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

    private static async
        Task<Results<Created<ImageUploadResultDto>, BadRequest<IEnumerable<string>>, InternalServerError>>
        UploadSharedImageAsync(
            [AsParameters] FogServices services,
            HttpContext context,
            ImageUploadDto content,
            CancellationToken ct)
    {
        var command = new UploadSharedImageCommand(content);
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

        return TypedResults.Created(dto.Url, dto);
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

    private static async Task<Ok<IReadOnlyCollection<CommunityCityStrategyDto>>> GetCommunityCityStrategiesAsync(
        [AsParameters] StatsServices services,
        CancellationToken ct)
    {
        var query = new GetCommunityCityStrategiesQuery();
        var result = await services.Mediator.Send(query, ct);
        result.LogIfFailed(nameof(FogApi));

        return TypedResults.Ok(result.IsSuccess ? result.Value : []);
    }

    private static async Task<Ok<IReadOnlyCollection<CommunityCityGuideInfoDto>>> GetCommunityCityGuidesAsync(
        [AsParameters] StatsServices services,
        CancellationToken ct)
    {
        var query = new GetCommunityCityGuidesQuery();
        var result = await services.Mediator.Send(query, ct);
        result.LogIfFailed(nameof(FogApi));

        return TypedResults.Ok(result.IsSuccess ? result.Value : []);
    }

    private static async Task<Results<Ok<CommunityCityGuideDto>, NotFound, InternalServerError>>
        GetCommunityCityGuideAsync([AsParameters] StatsServices services, HttpContext context,
            [AsParameters] GetCommunityCityGuideQuery query, CancellationToken ct = default)
    {
        var result = await services.Mediator.Send(query, ct);
        result.LogIfFailed(nameof(FogApi));
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
}
