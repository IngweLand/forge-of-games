using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;

public interface IBattleUiService
{
    Task<BattleStatsViewModel> GetBattleStatsAsync(int battleStatsId, CancellationToken ct = default);
    Task<BattleSelectorViewModel> GetBattleSelectorViewModel();

    Task<IReadOnlyCollection<UnitBattleViewModel>> GetUnitBattlesAsync(string unitId, BattleType battleType,
        CancellationToken ct = default);

    IReadOnlyCollection<UnitBattleTypeViewModel> GetUnitBattleTypes();

    Task<PaginatedList<BattleSummaryViewModel>> SearchBattles(
        UserBattleSearchRequest request, CancellationToken ct = default);

    Task<BattleSummaryViewModel?> GetBattleAsync(int battleId, CancellationToken ct = default);
}
