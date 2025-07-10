using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;

public interface IStatsHubUiService
{
    Task<PlayerWithRankingsViewModel?> GetPlayerAsync(int playerId);

    Task<PaginatedList<PlayerViewModel>> GetPlayerStatsAsync(string worldId, int startIndex, int pageSize,
        string? playerName = null, CancellationToken ct = default);

    Task<TopStatsViewModel> GetTopStatsAsync();

    Task<AllianceWithRankingsViewModel?> GetAllianceAsync(int allianceId);

    Task<PaginatedList<AllianceViewModel>> GetAllianceStatsAsync(string worldId, int startIndex, int pageSize,
        string? allianceName = null, CancellationToken ct = default);

    Task<BattleSelectorViewModel> GetBattleSelectorViewModel();

    Task<IReadOnlyCollection<BattleSummaryViewModel>> SearchBattles(BattleSearchRequest request,
        CancellationToken ct = default);

    Task<BattleStatsViewModel> GetBattleStatsAsync(int battleStatsId, CancellationToken ct = default);
    Task<IReadOnlyCollection<UnitBattleViewModel>> GetUnitBattlesAsync(string unitId, CancellationToken ct = default);
}
