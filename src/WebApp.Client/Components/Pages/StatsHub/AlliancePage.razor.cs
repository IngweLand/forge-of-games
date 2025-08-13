using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Core.Helpers;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class AlliancePage : StatsHubPageBase
{
    private AllianceWithRankingsViewModel? _alliance;

    private bool _canShowChart;
    private Dictionary<string, object> _defaultAnalyticsParameters = [];
    private bool _rankingChartIsExpanded;

    [Parameter]
    public required int AllianceId { get; set; }

    [Inject]
    public IAlliancePageAnalyticsService AnalyticsService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _alliance = await LoadWithPersistenceAsync(nameof(_alliance),
            () => StatsHubUiService.GetAllianceAsync(AllianceId));

        if (_alliance != null)
        {
            _defaultAnalyticsParameters = new Dictionary<string, object>
            {
                {AnalyticsParams.FOG_ALLIANCE_ID, _alliance.Alliance.Id},
                {AnalyticsParams.LOCATION, AnalyticsParams.Values.Locations.ALLIANCE_PROFILE},
            };
        }

        if (OperatingSystem.IsBrowser())
        {
            _canShowChart = true;
            IsInitialized = true;
        }
    }

    private void OnMemberClicked(int playerId)
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.Player(playerId));
    }

    private void ToggleRankingChart()
    {
        _rankingChartIsExpanded = !_rankingChartIsExpanded;

        AnalyticsService.TrackChartView(AnalyticsEvents.TOGGLE_CHART, _defaultAnalyticsParameters,
            AnalyticsParams.Values.Sources.ALLIANCE_RANKING_CHART, _rankingChartIsExpanded);
    }
}
