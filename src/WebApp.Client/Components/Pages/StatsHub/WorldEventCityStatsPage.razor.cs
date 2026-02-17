using Ingweland.Fog.Application.Client.Core.Localization;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Models.Fog;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class WorldEventCityStatsPage : WorldStatsPageBase<PlayerViewModel>
{
    protected override string GetTitle()
    {
        return WorldId == "zz1"
            ? Loc[FogResource.StatsHub_Worlds_TopEventCityListTitle, FogResource.StatsHub_Worlds_BetaWorld]
            : Loc[FogResource.StatsHub_Worlds_TopEventCityListTitle, FogResource.StatsHub_Worlds_MainWorld];
    }

    protected override async ValueTask<PaginatedList<PlayerViewModel>> FetchDataAsync(int startIndex, int count,
        string? query = null, CancellationToken ct = default)
    {
        return await StatsHubUiService.GetEventCityRankingsAsync(WorldId, ct);
    }
}
