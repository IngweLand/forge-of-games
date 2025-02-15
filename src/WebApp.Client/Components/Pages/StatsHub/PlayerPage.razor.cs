using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class PlayerPage : StatsHubPageBase
{
    private bool _canShowChart;
    private PlayerWithRankingsViewModel? _player;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!ApplicationState.TryTakeFromJson<PlayerWithRankingsViewModel?>(
                nameof(_player), out var restoredPlayer))
        {
            _player = await StatsHubUiService.GetPlayerAsync(WorldId, PlayerId);
        }
        else
        {
            if (restoredPlayer != null)
            {
                _player = restoredPlayer;
            }
            else
            {
                _player = await StatsHubUiService.GetPlayerAsync(WorldId, PlayerId);
            }
        }

        if (OperatingSystem.IsBrowser())
        {
            await IJsInteropService.RemoveLoadingIndicatorAsync();
            _canShowChart = true;
            IsInitialized = true;
        }
    }

    protected override Task PersistData()
    {
        ApplicationState.PersistAsJson(nameof(_player), _player);

        return Task.CompletedTask;
    }
}
