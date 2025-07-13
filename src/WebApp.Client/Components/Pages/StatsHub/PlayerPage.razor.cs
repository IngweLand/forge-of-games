using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.WebApp.Client.Components.Elements.StatsHub;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class PlayerPage : StatsHubPageBase, IAsyncDisposable
{
    private CancellationTokenSource _battleStatsCts = new();
    private bool _canShowChart;
    private CancellationTokenSource _cityFetchCts = new();
    private bool _fetchingCity;
    private bool _isDisposed;
    private PlayerWithRankingsViewModel? _player;

    [Inject]
    private CityPlannerNavigationState CityPlannerNavigationState { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }

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
            _player = await LoadWithPersistenceAsync(nameof(_player),
                () => StatsHubUiService.GetPlayerAsync(PlayerId));
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

    private void SearchAlliance(string allianceName)
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.SearchAlliance(_player!.Player.WorldId, allianceName));
    }

    private void OnPlayerClicked(int playerId)
    {
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

        _battleStatsCts = new CancellationTokenSource();
        var stats = await StatsHubUiService.GetBattleStatsAsync(battleStatsId.Value, _battleStatsCts.Token);

        if (_isDisposed)
        {
            return;
        }

        var options = GetDefaultDialogOptions();

        var parameters = new DialogParameters<BattleStatsDialog> {{d => d.Stats, stats}};
        await DialogService.ShowAsync<BattleStatsDialog>(null, parameters, options);
    }

    private async Task HandleCityOperation(Func<HohCity, Task> cityHandler)
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
            var city = await StatsHubService.GetPlayerCityAsync(_player!.Player.Id);
            if (_isDisposed || city == null)
            {
                return;
            }

            await cityHandler(city);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        if (_isDisposed)
        {
            return;
        }

        _fetchingCity = false;
    }

    private async Task VisitCity()
    {
        await HandleCityOperation(city =>
        {
            CityPlannerNavigationState.City = city;
            NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.CITY_PLANNER_APP_PATH);
            return Task.CompletedTask;
        });
    }

    private async Task ShowCityStats()
    {
        await HandleCityOperation(async city =>
        {
            city.Id = Guid.NewGuid().ToString("N");
            await PersistenceService.SaveTempCities([city]);
            NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.CITIES_STATS_PATH);
        });
    }
    
    private async Task OpenBattleSquadProfile(BattleSquadViewModel squad)
    {
        var options = GetDefaultDialogOptions();

        var parameters = new DialogParameters<BattleSquadProfileDialog> {{d => d.HeroProfile, squad}};
        await DialogService.ShowAsync<BattleSquadProfileDialog>(null, parameters, options);
    }
}
