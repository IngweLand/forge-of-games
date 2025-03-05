using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class StatsHubPage : StatsHubPageBase
{
    private TopStatsViewModel? _topStats;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _topStats = await LoadWithPersistenceAsync(nameof(_topStats),
            async () => await StatsHubUiService.GetTopStatsAsync());
        
        IsInitialized = true;
    }
}
