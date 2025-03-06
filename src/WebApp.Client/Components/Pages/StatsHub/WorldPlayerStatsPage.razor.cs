using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class WorldPlayerStatsPage : WorldStatsPageBase<PlayerViewModel>
{
    protected override string GetTitle()
    {
        return WorldId == "zz1"
            ? Loc[FogResource.StatsHub_Worlds_PlayerListTitle, FogResource.StatsHub_Worlds_BetaWorld]
            : Loc[FogResource.StatsHub_Worlds_PlayerListTitle, FogResource.StatsHub_Worlds_MainWorld];
    }

    protected override Task<PaginatedList<PlayerViewModel>> GetData(int pageNumber)
    {
        Cts?.Cancel();
        Cts = new CancellationTokenSource();
        
        return StatsHubUiService.GetPlayerStatsAsync(WorldId, pageNumber, Name, Cts.Token);
    }

    protected override void NavigateToItemPage(PlayerViewModel item)
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.Player(item.Id));
    }
   
}
