using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface IBattleService
{
    [Post(FogUrlBuilder.ApiRoutes.BATTLE_LOG_SEARCH)]
    Task<BattleSearchResult> SearchBattlesAsync([Body] BattleSearchRequest request, CancellationToken ct = default);

    [Get(FogUrlBuilder.ApiRoutes.BATTLE_STATS_TEMPLATE_REFIT)]
    Task<BattleStatsDto?> GetBattleStatsAsync(int battleStatsId, CancellationToken ct = default);

}
