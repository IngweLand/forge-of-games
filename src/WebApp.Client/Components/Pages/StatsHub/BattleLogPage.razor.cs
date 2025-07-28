using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.WebApp.Client.Components.Elements.StatsHub;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class BattleLogPage : StatsHubPageBase, IAsyncDisposable
{
    private readonly Dictionary<string, object> _defaultAnalyticsParameters = new()
    {
        {AnalyticsParams.LOCATION, AnalyticsParams.Values.Locations.BATTLE_LOG},
    };

    private IReadOnlyCollection<BattleSummaryViewModel> _battles = [];

    private CancellationTokenSource _battlesCts = new();
    private BattleSearchRequest _battleSearchRequest = new();

    private BattleSelectorViewModel? _battleSelectorViewModel;
    private CancellationTokenSource _battleStatsCts = new();

    private bool _isDisposed;

    private bool _isLoading = true;

    [Inject]
    public IBattleLogPageAnalyticsService AnalyticsService { get; set; }

    [Inject]
    private IBattleSearchRequestFactory BattleSearchRequestFactory { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _battleSelectorViewModel = await LoadWithPersistenceAsync(
            nameof(_battleSelectorViewModel),
            async () => await StatsHubUiService.GetBattleSelectorViewModel()
        );
    }

    protected override async Task OnParametersSetAsync()
    {
        if (_isDisposed)
        {
            return;
        }

        await base.OnParametersSetAsync();
        await GetBattles(BattleSearchRequestFactory.Create(NavigationManager.Uri));
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;

        await _battlesCts.CancelAsync();
        _battlesCts.Dispose();

        await _battleStatsCts.CancelAsync();
        _battleStatsCts.Dispose();
    }

    private async Task GetBattles(BattleSearchRequest request)
    {
        if (_isDisposed)
        {
            return;
        }

        _isLoading = true;
        _battles = [];

        await _battlesCts.CancelAsync();
        _battlesCts.Dispose();

        if (_isDisposed)
        {
            return;
        }

        _battlesCts = new CancellationTokenSource();
        _battleSearchRequest = request;

        try
        {
            _battles = await StatsHubUiService.SearchBattles(request, _battlesCts.Token);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
        }

        if (_isDisposed)
        {
            return;
        }

        _isLoading = false;
    }

    private Task BattleSelectorOnValueChanged(BattleSearchRequest newValue)
    {
        AnalyticsService.TrackFormChange(newValue, _defaultAnalyticsParameters);

        return GetBattles(newValue);
    }

    private void OnContributionPromptClick()
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.HELP_BATTLE_LOG_PATH);
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

    private async Task OpenBattleStats(BattleSummaryViewModel battle)
    {
        if (_isDisposed || battle.StatsId == null)
        {
            return;
        }

        AnalyticsService.TrackEvent(AnalyticsEvents.VIEW_BATTLE_STATS, _defaultAnalyticsParameters,
            new Dictionary<string, object> {{AnalyticsParams.FOG_BATTLE_ID, battle.StatsId.Value}});

        await _battleStatsCts.CancelAsync();
        _battleStatsCts.Dispose();

        if (_isDisposed)
        {
            return;
        }

        _battleStatsCts = new CancellationTokenSource();
        var stats = await StatsHubUiService.GetBattleStatsAsync(battle.StatsId.Value, _battleStatsCts.Token);
        var options = GetDefaultDialogOptions();

        var parameters = new DialogParameters<BattleStatsDialog> {{d => d.Stats, stats}};
        await DialogService.ShowAsync<BattleStatsDialog>(null, parameters, options);
    }

    private async Task OpenBattleSquadProfile(BattleSquadViewModel squad)
    {
        AnalyticsService.TrackSquadProfileView(AnalyticsEvents.VIEW_SQUAD_PROFILE, _defaultAnalyticsParameters,
            squad.HeroUnitId);

        var options = GetDefaultDialogOptions();

        var parameters = new DialogParameters<ProfileSquadDialog> {{d => d.HeroProfile, squad}};
        await DialogService.ShowAsync<ProfileSquadDialog>(null, parameters, options);
    }
}
