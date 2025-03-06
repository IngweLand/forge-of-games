using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Application.Server.StatsHub.Queries;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ingweland.Fog.WebApp.Apis;

public static class StatsApi
{
    public static RouteGroupBuilder MapStatsApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/hoh");

        api.MapGet(FogUrlBuilder.ApiRoutes.PLAYERS_TEMPLATE, GetPlayersAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.PLAYER_TEMPLATE, GetPlayerAsync);
        
        api.MapGet(FogUrlBuilder.ApiRoutes.ALLIANCES_TEMPLATE, GetAlliancesAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.ALLIANCE_TEMPLATE, GetAllianceAsync);

        return api;
    }

    private static async Task<Results<Ok<PlayerWithRankings>, NotFound, BadRequest<string>>>
        GetPlayerAsync([AsParameters] StatsServices services, HttpContext context,
            string worldId, int inGamePlayerId)
    {
        var query = new GetPlayerQuery()
        {
            PlayerKey = new PlayerKey(worldId, inGamePlayerId),
        };
        var result = await services.Mediator.Send(query);
        if (result == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<PaginatedList<PlayerDto>>, BadRequest<string>>>
        GetPlayersAsync([AsParameters] StatsServices services, HttpContext context,
            string worldId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = FogConstants.DEFAULT_STATS_PAGE_SIZE,
            [FromQuery] string? name = null)
    {
        var query = new GetPlayersWithPaginationQuery()
        {
            PageNumber = pageNumber, PageSize = pageSize, WorldId = worldId, PlayerName = name,
        };
        var result = await services.Mediator.Send(query);

        return TypedResults.Ok(result);
    }
    
    private static async Task<Results<Ok<AllianceWithRankings>, NotFound, BadRequest<string>>>
        GetAllianceAsync([AsParameters] StatsServices services, HttpContext context,
            string worldId, int inGameAllianceId)
    {
        var query = new GetAllianceQuery()
        {
            AllianceKey = new AllianceKey(worldId, inGameAllianceId),
        };
        var result = await services.Mediator.Send(query);
        if (result == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<PaginatedList<AllianceDto>>, BadRequest<string>>>
        GetAlliancesAsync([AsParameters] StatsServices services, HttpContext context,
            string worldId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = FogConstants.DEFAULT_STATS_PAGE_SIZE,
            [FromQuery] string? name = null)
    {
        var query = new GetAlliancesWithPaginationQuery()
        {
            PageNumber = pageNumber, PageSize = pageSize, WorldId = worldId, AllianceName = name,
        };
        var result = await services.Mediator.Send(query);

        return TypedResults.Ok(result);
    }
}
