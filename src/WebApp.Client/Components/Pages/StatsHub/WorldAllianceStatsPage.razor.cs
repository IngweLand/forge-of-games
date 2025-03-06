using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class WorldAllianceStatsPage : WorldStatsPageBase<AllianceViewModel>
{
    protected override string GetTitle()
    {
        return WorldId == "zz1"
            ? Loc[FogResource.StatsHub_Worlds_AllianceListTitle, FogResource.StatsHub_Worlds_BetaWorld]
            : Loc[FogResource.StatsHub_Worlds_AllianceListTitle, FogResource.StatsHub_Worlds_MainWorld];
    }

    protected override Task<PaginatedList<AllianceViewModel>> GetData(int pageNumber)
    {
        Cts?.Cancel();
        Cts = new CancellationTokenSource();
        
        return StatsHubUiService.GetAllianceStatsAsync(WorldId, pageNumber, Name, Cts.Token);
    }

    protected override void NavigateToItemPage(AllianceViewModel item)
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.Alliance(item.Key.WorldId, item.Key.InGameAllianceId));
    }
   
}
