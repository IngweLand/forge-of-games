using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Models.Fog;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;

public interface IStatsHubUiService
{
    Task<PlayerWithRankingsViewModel?> GetPlayerAsync(string worldId, int playerId);

    Task<PaginatedList<PlayerViewModel>> GetPlayerStatsAsync(string worldId, int pageNumber = 1,
        string? playerName = null, CancellationToken ct = default);

    Task<TopStatsViewModel> GetTopStatsAsync();

    Task<AllianceWithRankingsViewModel?> GetAllianceAsync(string worldId, int allianceId);

    Task<PaginatedList<AllianceViewModel>> GetAllianceStatsAsync(string worldId, int pageNumber = 1,
        string? allianceName = null, CancellationToken ct = default);
}