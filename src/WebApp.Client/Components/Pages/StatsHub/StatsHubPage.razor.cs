using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class StatsHubPage : StatsHubPageBase
{
    private TopStatsViewModel? _topStats;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!ApplicationState.TryTakeFromJson<TopStatsViewModel>(
                nameof(_topStats), out var restoredTopStats))
        {
            _topStats = await StatsHubUiService.GetTopStatsAsync();
        }
        else
        {
            _topStats = restoredTopStats;
        }

        if (OperatingSystem.IsBrowser())
        {
            await IJsInteropService.RemoveLoadingIndicatorAsync();
            IsInitialized = true;
        }
    }

    protected override Task PersistData()
    {
        ApplicationState.PersistAsJson(nameof(_topStats), _topStats);

        return Task.CompletedTask;
    }
}
