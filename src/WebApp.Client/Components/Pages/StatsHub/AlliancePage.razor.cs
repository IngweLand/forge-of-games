using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Microsoft.AspNetCore.Components;
using Refit;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class AlliancePage : StatsHubPageBase
{
    private AllianceWithRankingsViewModel? _alliance;
    private IReadOnlyCollection<AllianceAthRankingViewModel>? _athRankings;
    private bool _athRankingsAreLoading;
    private bool _athRankingsContainerIsExpanded;
    private CancellationTokenSource? _athRankingsCts;

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

    private void ToggleRankingChart()
    {
        _rankingChartIsExpanded = !_rankingChartIsExpanded;

        AnalyticsService.TrackChartView(AnalyticsEvents.TOGGLE_CHART, _defaultAnalyticsParameters,
            AnalyticsParams.Values.Sources.ALLIANCE_RANKING_CHART, _rankingChartIsExpanded);
    }

    private async Task ToggleAthRankingsContainer()
    {
        _athRankingsContainerIsExpanded = !_athRankingsContainerIsExpanded;
        
        AnalyticsService.TrackChartView(AnalyticsEvents.TOGGLE_VIEW, _defaultAnalyticsParameters,
            AnalyticsParams.Values.Sources.ALLIANCE_ATH_RANKINGS, _athRankingsContainerIsExpanded);
        
        await GetAthRankings();
    }

    private async Task GetAthRankings()
    {
        if (_athRankings != null)
        {
            return;
        }

        if (_athRankingsCts != null)
        {
            await _athRankingsCts.CancelAsync();
        }
        
        _athRankingsAreLoading = true;
        StateHasChanged();

        _athRankingsCts = new CancellationTokenSource();

        try
        {
            _athRankings = await StatsHubUiService.GetAllianceAthRankingsAsync(AllianceId);
            _athRankingsAreLoading = false;
        }
        catch (OperationCanceledException _)
        {
        }
        catch (ApiException apiEx) when (apiEx.InnerException is TaskCanceledException)
        {
            _athRankingsAreLoading = false;
        }
        catch (Exception e)
        {
            _athRankingsAreLoading = false;
            Console.Error.WriteLine(e);
        }
    }
}
