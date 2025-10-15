using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Shared.Extensions;
using Ingweland.Fog.WebApp.Client.Components.Elements.StatsHub;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class PlayerProfilePage : StatsHubPageBase, IAsyncDisposable
{
    private CancellationTokenSource _battleStatsCts = new();
    private bool _canShowChart;
    private CancellationTokenSource _cityFetchCts = new();
    private Dictionary<string, object> _defaultAnalyticsParameters = [];
    private bool _fetchingCity;
    private bool _isDisposed;
    private PlayerProfileViewModel? _player;
    private bool _pvpChartIsExpanded;
    private bool _rankingChartIsExpanded;
    private DateTime? _citySnapshotDate = DateTime.Today;

    [Inject]
    public IPlayerProfilePageAnalyticsService AnalyticsService { get; set; }

    [Inject]
    private IBattleUiService BattleUiService { get; set; }

    [Inject]
    private CityPlannerNavigationState CityPlannerNavigationState { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }

    [Inject]
    private ILogger<PlayerProfilePage> Logger { get; set; }

    [Inject]
    private IPersistenceService PersistenceService { get; set; }

    [Inject]
    private IStatsHubService StatsHubService { get; set; }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (OperatingSystem.IsBrowser())
        {
            _canShowChart = true;
            IsInitialized = true;
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        if (_isDisposed)
        {
            return;
        }

        await base.OnParametersSetAsync();

        if (_player == null || _player.Player.Id != PlayerId)
        {
            IsInitialized = false;
            _player = await StatsHubUiService.GetPlayerProfileAsync(PlayerId);
            _citySnapshotDate = _player!.CitySnapshotDays.Last();

            _defaultAnalyticsParameters = new Dictionary<string, object>
            {
                {AnalyticsParams.FOG_PLAYER_ID, _player!.Player.Id},
                {AnalyticsParams.LOCATION, AnalyticsParams.Values.Locations.PLAYER_PROFILE},
            };

            IsInitialized = true;
        }
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;

        await _cityFetchCts.CancelAsync();
        _cityFetchCts.Dispose();

        await _battleStatsCts.CancelAsync();
        _battleStatsCts.Dispose();
    }

    private void OnPlayerClicked(int playerId)
    {
        AnalyticsService.TrackEvent(AnalyticsEvents.NAVIGATE_PLAYER_PROFILE, _defaultAnalyticsParameters,
            new Dictionary<string, object> {{AnalyticsParams.SOURCE, AnalyticsParams.Values.Sources.PVP_BATTLE}});

        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.Player(playerId));
    }

    private static DialogOptions GetDefaultDialogOptions()
    {
        return new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            BackgroundClass = "dialog-blur-bg",
            Position = DialogPosition.TopCenter,
            CloseButton = true,
            CloseOnEscapeKey = true,
            NoHeader = true,
        };
    }

    private async Task OpenBattleStats(int? battleStatsId)
    {
        await _battleStatsCts.CancelAsync();
        _battleStatsCts.Dispose();
        if (battleStatsId == null)
        {
            return;
        }

        AnalyticsService.TrackEvent(AnalyticsEvents.VIEW_BATTLE_STATS, _defaultAnalyticsParameters,
            new Dictionary<string, object> {{AnalyticsParams.FOG_BATTLE_ID, battleStatsId}});

        _battleStatsCts = new CancellationTokenSource();
        var stats = await BattleUiService.GetBattleStatsAsync(battleStatsId.Value, _battleStatsCts.Token);

        if (_isDisposed)
        {
            return;
        }

        var options = GetDefaultDialogOptions();

        var parameters = new DialogParameters<BattleStatsDialog> {{d => d.Stats, stats}};
        await DialogService.ShowAsync<BattleStatsDialog>(null, parameters, options);
    }

    private async Task HandleCityOperation(Func<HohCity, Task> cityHandler, Action onFailure)
    {
        if (_isDisposed)
        {
            return;
        }

        await _cityFetchCts.CancelAsync();
        _cityFetchCts.Dispose();

        if (_isDisposed)
        {
            return;
        }

        _fetchingCity = true;
        _cityFetchCts = new CancellationTokenSource();

        try
        {
            var city = await StatsHubService.GetPlayerCityAsync(_player!.Player.Id, _citySnapshotDate?.ToDateOnly());
            if (_isDisposed)
            {
                return;
            }

            if (city == null)
            {
                onFailure.Invoke();
                return;
            }

            await cityHandler(city);
        }
        catch (Exception e)
        {
            onFailure.Invoke();
            Logger.LogError(e, "Error while fetching city data");
        }

        if (_isDisposed)
        {
            return;
        }

        _fetchingCity = false;
    }

    private async Task VisitCity()
    {
        AnalyticsService.TrackEvent(AnalyticsEvents.VISIT_CITY_INIT, _defaultAnalyticsParameters);

        await HandleCityOperation(city =>
            {
                AnalyticsService.TrackEvent(AnalyticsEvents.VISIT_CITY_SUCCESS, _defaultAnalyticsParameters);

                CityPlannerNavigationState.City = city;
                NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.CITY_PLANNER_APP_PATH);
                return Task.CompletedTask;
            },
            () => AnalyticsService.TrackEvent(AnalyticsEvents.VISIT_CITY_ERROR, _defaultAnalyticsParameters));
    }

    private async Task ShowCityStats()
    {
        AnalyticsService.TrackEvent(AnalyticsEvents.VIEW_CITY_STATS_INIT, _defaultAnalyticsParameters);

        await HandleCityOperation(async city =>
            {
                AnalyticsService.TrackEvent(AnalyticsEvents.VIEW_CITY_STATS_SUCCESS, _defaultAnalyticsParameters);

                city.Id = Guid.NewGuid().ToString("N");
                await PersistenceService.SaveTempCities([city]);
                NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.CITIES_STATS_PATH);
            },
            () => AnalyticsService.TrackEvent(AnalyticsEvents.VIEW_CITY_STATS_ERROR, _defaultAnalyticsParameters));
    }

    private void ToggleRankingChart()
    {
        _rankingChartIsExpanded = !_rankingChartIsExpanded;

        AnalyticsService.TrackChartView(AnalyticsEvents.TOGGLE_CHART, _defaultAnalyticsParameters,
            AnalyticsParams.Values.Sources.PLAYER_RANKING_CHART, _rankingChartIsExpanded);
    }

    private void TogglePvpChart()
    {
        _pvpChartIsExpanded = !_pvpChartIsExpanded;

        AnalyticsService.TrackChartView(AnalyticsEvents.TOGGLE_CHART, _defaultAnalyticsParameters,
            AnalyticsParams.Values.Sources.PLAYER_PVP_RANKING_CHART, _pvpChartIsExpanded);
    }

    private async Task OpenBattleSquadProfile(HeroProfileViewModel squad)
    {
        var options = GetDefaultDialogOptions();

        var parameters = new DialogParameters<ProfileSquadDialog> {{d => d.HeroProfile, squad}};
        await DialogService.ShowAsync<ProfileSquadDialog>(null, parameters, options);
    }

    private void NavigateToBattlesScreen()
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.PlayerBattles(PlayerId));
    }

    private void OnPlayerInfoAllianceClicked(int allianceId)
    {
        AnalyticsService.TrackAllianceNavigation(AnalyticsEvents.NAVIGATE_ALLIANCE, _defaultAnalyticsParameters,
            AnalyticsParams.Values.Sources.PLAYER_INFO_COMPONENT, allianceId);
    }

    private void OnAllianceClicked(int allianceId)
    {
        AnalyticsService.TrackAllianceNavigation(AnalyticsEvents.NAVIGATE_ALLIANCE, _defaultAnalyticsParameters,
            AnalyticsParams.Values.Sources.ALLIANCE_LIST, allianceId);
    }

    private async Task OnProfileSquadClicked(HeroProfileBasicViewModel profile)
    {
        AnalyticsService.TrackSquadProfileView(AnalyticsEvents.VIEW_SQUAD_PROFILE, _defaultAnalyticsParameters,
            AnalyticsParams.Values.Sources.TOP_HEROES, profile.HeroUnitId);

        var fullProfile = await BattleUiService.CreateHeroProfile(profile);
        await OpenBattleSquadProfile(fullProfile);
    }
    
    private async Task OnPvpBattleSquadClicked(BattleSquadBasicViewModel squad)
    {
        AnalyticsService.TrackSquadProfileView(AnalyticsEvents.VIEW_SQUAD_PROFILE, _defaultAnalyticsParameters,
            AnalyticsParams.Values.Sources.PVP_BATTLE, squad.HeroUnitId);
        
        var fullSquad = await BattleUiService.CreateHeroProfile(squad);
        await OpenBattleSquadProfile(fullSquad);
    }
}
