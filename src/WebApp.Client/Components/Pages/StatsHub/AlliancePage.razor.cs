using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Core.Helpers;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class AlliancePage : StatsHubPageBase
{
    private bool _canShowChart;
    private AllianceWithRankingsViewModel? _alliance;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        _alliance = await LoadWithPersistenceAsync(nameof(_alliance),
            () => StatsHubUiService.GetAllianceAsync(WorldId, AllianceId));

        if (OperatingSystem.IsBrowser())
        {
            _canShowChart = true;
            IsInitialized = true;
        }
    }
    
    private void OnPlayerClicked(PlayerViewModel player)
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.Player(player.Key.WorldId, player.Key.InGamePlayerId));
    }
}
