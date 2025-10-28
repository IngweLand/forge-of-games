using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Application.Server.PlayerCity.Queries;
using Ingweland.Fog.Application.Server.StatsHub.Queries;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.PlayerCity;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ingweland.Fog.WebApp.Apis;

public static class StatsApi
{
    public static RouteGroupBuilder MapStatsApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/hoh");

        api.MapGet(FogUrlBuilder.ApiRoutes.ALL_LEADERBOARD_TOP_ITEMS_PATH, GetAllLeaderboardTopItemsAsync);

        api.MapGet(FogUrlBuilder.ApiRoutes.TOP_HEROES_PATH, GetTopHeroesAsync);

        api.MapGet(FogUrlBuilder.ApiRoutes.PLAYERS_TEMPLATE, GetPlayersAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.PLAYER_PROFILE_TEMPLATE, GetPlayerProfileAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.PLAYER_CITY_TEMPLATE, GetPlayerCityAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.PLAYER_TEMPLATE, GetPlayerAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.PLAYER_BATTLES_TEMPLATE, GetPlayerBattlesAsync);

        api.MapGet(FogUrlBuilder.ApiRoutes.ALLIANCES_TEMPLATE, GetAlliancesAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.ALLIANCE_TEMPLATE, GetAllianceAsync);

        api.MapPost(FogUrlBuilder.ApiRoutes.BATTLE_LOG_SEARCH, SearchBattlesAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.BATTLE_STATS_TEMPLATE, GetBattleStatsAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.BATTLE_TEMPLATE, GetBattleAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.UNIT_BATTLES_TEMPLATE, GetUnitBattlesAsync);
        api.MapPost(FogUrlBuilder.ApiRoutes.PLAYER_CITY_SNAPSHOTS_SEARCH, SearchCityInspirationsAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.PLAYER_CITY_SNAPSHOT_TEMPLATE, GetPlayerCitySnapshotAsync);

        api.MapPost(FogUrlBuilder.ApiRoutes.USER_BATTLE_SEARCH, SearchUserBattlesAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.EQUIPMENT_INSIGHTS_TEMPLATE, GetEquipmentInsightsAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.RELICS_INSIGHTS_TEMPLATE, GetRelicInsightsAsync);

        return api;
    }

    private static async Task<Results<Ok<PlayerProfileDto>, NotFound, BadRequest<string>>>
        GetPlayerProfileAsync([AsParameters] StatsServices services, HttpContext context, int playerId,
            CancellationToken ct = default)
    {
        var query = new GetPlayerProfileQuery
        {
            PlayerId = playerId,
        };
        var result = await services.Mediator.Send(query, ct);
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

    private static async Task<Results<Ok<IReadOnlyCollection<string>>, BadRequest<string>>>
        GetTopHeroesAsync([AsParameters] StatsServices services, HttpContext context,
            [AsParameters] GetTopHeroesQuery query, CancellationToken ct = default)
    {
        var result = await services.Mediator.Send(query, ct);

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<HohCity>, NotFound, BadRequest<string>>>
        GetPlayerCityAsync([AsParameters] StatsServices services, HttpContext context, int playerId,
            [FromQuery] DateOnly? date = null, CancellationToken ct = default)
    {
        var query = new GetPlayerCityQuery(playerId, date);
        var result = await services.Mediator.Send(query, ct);
        if (result == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<BattleStatsDto>, NotFound>>
        GetBattleStatsAsync([AsParameters] StatsServices services, HttpContext context, int battleStatsId,
            CancellationToken ct = default)
    {
        var result = await services.BattleService.GetBattleStatsAsync(battleStatsId, ct);
        if (result == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<BattleDto>, NotFound>>
        GetBattleAsync([AsParameters] StatsServices services, HttpContext context, int battleId,
            CancellationToken ct = default)
    {
        var result = await services.BattleService.GetBattleAsync(battleId, ct);
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
    
    private static async Task<Ok<IReadOnlyCollection<EquipmentInsightsDto>>>
        GetEquipmentInsightsAsync([AsParameters] StatsServices services, HttpContext context, string unitId,
            CancellationToken ct)
    {
        var result = await services.EquipmentService.GetInsightsAsync(unitId, ct);
        return TypedResults.Ok(result);
    }
    
    private static async Task<Ok<IReadOnlyCollection<RelicInsightsDto>>>
        GetRelicInsightsAsync([AsParameters] StatsServices services, HttpContext context, string unitId,
            CancellationToken ct)
    {
        var result = await services.RelicService.GetInsightsAsync(unitId, ct);
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<PlayerDto>, NotFound>>
        GetPlayerAsync([AsParameters] StatsServices services, HttpContext context, int playerId,
            CancellationToken ct)
    {
        var result = await services.StatsHubService.GetPlayerAsync(playerId, ct);
        if (result == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result);
    }

    private static async Task<Ok<PaginatedList<PvpBattleDto>>>
        GetPlayerBattlesAsync([AsParameters] StatsServices services, HttpContext context,
            [AsParameters] GetPlayerBattlesQuery query,
            CancellationToken ct)
    {
        var result = await services.Mediator.Send(query, ct);

        return TypedResults.Ok(result);
    }

    private static async Task<Ok<IReadOnlyCollection<UnitBattleDto>>>
        GetUnitBattlesAsync([AsParameters] StatsServices services, HttpContext context, string unitId,
            BattleType battleType, CancellationToken ct = default)
    {
        var result = await services.BattleService.GetUnitBattlesAsync(unitId, battleType, ct);

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
        GetAllianceAsync([AsParameters] StatsServices services, HttpContext context, int allianceId,
            CancellationToken ct = default)
    {
        var query = new GetAllianceQuery
        {
            AllianceId = allianceId,
        };
        var result = await services.Mediator.Send(query, ct);
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
            [FromBody] BattleSearchRequest request, CancellationToken ct = default)
    {
        var result = await services.BattleService.SearchBattlesAsync(request, ct);

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<PaginatedList<BattleSummaryDto>>, NotFound, BadRequest<string>>>
        SearchUserBattlesAsync([AsParameters] StatsServices services, HttpContext context,
            [FromBody] UserBattleSearchRequest request, CancellationToken ct = default)
    {
        var result = await services.BattleService.SearchBattlesAsync(request, ct);

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<IReadOnlyCollection<PlayerCitySnapshotBasicDto>>, BadRequest<string>>>
        SearchCityInspirationsAsync([AsParameters] StatsServices services, HttpContext context,
            [FromBody] CityInspirationsSearchRequest request, CancellationToken ct = default)
    {
        var result = await services.CityPlannerService.GetInspirationsAsync(request, ct);

        return TypedResults.Ok(result);
    }
}
