using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Models.Fog;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class WorldAllianceStatsPage : WorldStatsPageBase<AllianceViewModel>
{
    protected override string GetTitle()
    {
        return WorldId == "zz1"
            ? Loc[FogResource.StatsHub_Worlds_AllianceListTitle, FogResource.StatsHub_Worlds_BetaWorld]
            : Loc[FogResource.StatsHub_Worlds_AllianceListTitle, FogResource.StatsHub_Worlds_MainWorld];
    }

    protected override async ValueTask<PaginatedList<AllianceViewModel>> FetchDataAsync(ItemsProviderRequest request)
    {
        return await StatsHubUiService.GetAllianceStatsAsync(WorldId, request.StartIndex, request.Count, Name,
            request.CancellationToken);
    }
}
