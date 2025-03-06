using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface IStatsHubService
{
    [Get(FogUrlBuilder.ApiRoutes.PLAYER_TEMPLATE_REFIT)]
    Task<PlayerWithRankings?> GetPlayerAsync(string worldId, int inGamePlayerId);

    [Get(FogUrlBuilder.ApiRoutes.PLAYERS_TEMPLATE)]
    Task<PaginatedList<PlayerDto>> GetPlayersAsync(string worldId, [Query] int pageNumber = 1,
        [Query] int pageSize = FogConstants.DEFAULT_STATS_PAGE_SIZE, [Query] string? name = null,
        CancellationToken ct = default);
    
    [Get(FogUrlBuilder.ApiRoutes.ALLIANCE_TEMPLATE_REFIT)]
    Task<AllianceWithRankings?> GetAllianceAsync(string worldId, int inGameAllianceId);

    [Get(FogUrlBuilder.ApiRoutes.ALLIANCES_TEMPLATE)]
    Task<PaginatedList<AllianceDto>> GetAlliancesAsync(string worldId, [Query] int pageNumber = 1,
        [Query] int pageSize = FogConstants.DEFAULT_STATS_PAGE_SIZE, [Query] string? name = null,
        CancellationToken ct = default);
}
