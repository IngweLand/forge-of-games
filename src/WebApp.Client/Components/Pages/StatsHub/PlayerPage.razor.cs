using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class PlayerPage : StatsHubPageBase
{
    private bool _canShowChart;
    private PlayerWithRankingsViewModel? _player;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        _player = await LoadWithPersistenceAsync(nameof(_player),
            () => StatsHubUiService.GetPlayerAsync(WorldId, PlayerId));

        if (OperatingSystem.IsBrowser())
        {
            _canShowChart = true;
            IsInitialized = true;
        }
    }
}
