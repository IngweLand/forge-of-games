using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Application.Server.PlayerCity.Queries;
using Ingweland.Fog.Application.Server.StatsHub.Queries;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.PlayerCity;
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

        api.MapGet(FogUrlBuilder.ApiRoutes.ALL_LEADERBOARD_TOP_ITEMS_PATH, GetAllLeaderboardTopItemsAsync);

        api.MapGet(FogUrlBuilder.ApiRoutes.PLAYERS_TEMPLATE, GetPlayersAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.PLAYER_TEMPLATE, GetPlayerAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.PLAYER_CITY_TEMPLATE, GetPlayerCityAsync);

        api.MapGet(FogUrlBuilder.ApiRoutes.ALLIANCES_TEMPLATE, GetAlliancesAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.ALLIANCE_TEMPLATE, GetAllianceAsync);

        api.MapPost(FogUrlBuilder.ApiRoutes.BATTLE_LOG_SEARCH, SearchBattlesAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.BATTLE_STATS_TEMPLATE, GetBattleStatsAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.UNIT_BATTLES_TEMPLATE, GetUnitBattlesAsync);
        api.MapPost(FogUrlBuilder.ApiRoutes.PLAYER_CITY_SNAPSHOTS_SEARCH, SearchCityInspirationsAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.PLAYER_CITY_SNAPSHOT_TEMPLATE, GetPlayerCitySnapshotAsync);

        return api;
    }

    private static async Task<Results<Ok<PlayerWithRankings>, NotFound, BadRequest<string>>>
        GetPlayerAsync([AsParameters] StatsServices services, HttpContext context, int playerId)
    {
        var query = new GetPlayerQuery
        {
            PlayerId = playerId,
        };
        var result = await services.Mediator.Send(query);
        if (result == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<LeaderboardTopItemsDto>, BadRequest<string>>>
        GetAllLeaderboardTopItemsAsync([AsParameters] StatsServices services, HttpContext context,
            CancellationToken ct = default)
    {
        var result = await services.StatsHubService.GetAllLeaderboardTopItemsAsync(ct);

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<HohCity>, NotFound, BadRequest<string>>>
        GetPlayerCityAsync([AsParameters] StatsServices services, HttpContext context, int playerId)
    {
        var query = new GetPlayerCityQuery(playerId);
        var result = await services.Mediator.Send(query);
        if (result == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<BattleStatsDto>, NotFound>>
        GetBattleStatsAsync([AsParameters] StatsServices services, HttpContext context, int battleStatsId)
    {
        var result = await services.BattleService.GetBattleStatsAsync(battleStatsId);
        if (result == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<HohCity>, NotFound>>
        GetPlayerCitySnapshotAsync([AsParameters] StatsServices services, HttpContext context, int snapshotId,
            CancellationToken ct)
    {
        var result = await services.CityPlannerService.GetPlayerCitySnapshotAsync(snapshotId, ct);
        if (result == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result);
    }

    private static async Task<Ok<IReadOnlyCollection<UnitBattleDto>>>
        GetUnitBattlesAsync([AsParameters] StatsServices services, HttpContext context, string unitId)
    {
        var result = await services.BattleService.GetUnitBattlesAsync(unitId);

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<PaginatedList<PlayerDto>>, BadRequest<string>>>
        GetPlayersAsync([AsParameters] StatsServices services, HttpContext context,
            [AsParameters] GetPlayersWithPaginationQuery query, CancellationToken ct = default)
    {
        var result = await services.Mediator.Send(query, ct);

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<AllianceWithRankings>, NotFound, BadRequest<string>>>
        GetAllianceAsync([AsParameters] StatsServices services, HttpContext context, int allianceId)
    {
        var query = new GetAllianceQuery
        {
            AllianceId = allianceId,
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
            [AsParameters] GetAlliancesWithPaginationQuery query,
            CancellationToken ct = default)
    {
        var result = await services.Mediator.Send(query, ct);

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<BattleSearchResult>, NotFound, BadRequest<string>>>
        SearchBattlesAsync([AsParameters] StatsServices services, HttpContext context,
            [FromBody] BattleSearchRequest request)
    {
        var result = await services.BattleService.SearchBattlesAsync(request);

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<IReadOnlyCollection<PlayerCitySnapshotBasicDto>>, BadRequest<string>>>
        SearchCityInspirationsAsync([AsParameters] StatsServices services, HttpContext context,
            [FromBody] CityInspirationsSearchRequest request)
    {
        var result = await services.CityPlannerService.GetInspirationsAsync(request);

        return TypedResults.Ok(result);
    }
}
