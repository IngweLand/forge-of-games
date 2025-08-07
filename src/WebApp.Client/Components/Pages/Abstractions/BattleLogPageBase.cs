using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.WebApp.Client.Components.Elements.StatsHub;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;

public abstract class BattleLogPageBase : FogPageBase, IAsyncDisposable
{
    private CancellationTokenSource? _battleStatsCts;

    [Inject]
    protected IBattleLogPageAnalyticsService AnalyticsService { get; set; }

    protected BattleSearchRequest BattleSearchRequest { get; set; } = new();

    [Inject]
    protected IBattleUiService BattleUiService { get; set; }

    protected abstract Dictionary<string, object> DefaultAnalyticsParameters { get; }

    [Inject]
    protected IDialogService DialogService { get; set; }

    protected bool IsBrowser { get; set; }

    protected bool IsLoading { get; set; } = true;

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    protected BattleSelectorViewModel? SelectorViewModel { get; private set; }

    [Inject]
    protected IStatsHubUiService StatsHubUiService { get; set; }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }

    protected async Task OpenBattleStats(BattleSummaryViewModel battle)
    {
        if (battle.StatsId == null)
        {
            return;
        }

        AnalyticsService.TrackEvent(AnalyticsEvents.VIEW_BATTLE_STATS, DefaultAnalyticsParameters,
            new Dictionary<string, object> {{AnalyticsParams.FOG_BATTLE_ID, battle.StatsId.Value}});

        if (_battleStatsCts != null)
        {
            await _battleStatsCts.CancelAsync();
        }

        _battleStatsCts = new CancellationTokenSource();
        var stats = await BattleUiService.GetBattleStatsAsync(battle.StatsId.Value, _battleStatsCts.Token);
        var options = GetDefaultDialogOptions();

        var parameters = new DialogParameters<BattleStatsDialog> {{d => d.Stats, stats}};
        await DialogService.ShowAsync<BattleStatsDialog>(null, parameters, options);
    }

    protected async Task OpenBattleSquadProfile(BattleSquadViewModel squad)
    {
        AnalyticsService.TrackSquadProfileView(AnalyticsEvents.VIEW_SQUAD_PROFILE, DefaultAnalyticsParameters,
            squad.HeroUnitId);

        var options = GetDefaultDialogOptions();

        var parameters = new DialogParameters<ProfileSquadDialog> {{d => d.HeroProfile, squad}};
        await DialogService.ShowAsync<ProfileSquadDialog>(null, parameters, options);
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (OperatingSystem.IsBrowser())
        {
            await JsInteropService.ShowLoadingIndicatorAsync();
            StateHasChanged();
        }
        
        SelectorViewModel = await LoadWithPersistenceAsync(
            nameof(SelectorViewModel),
            async () => await BattleUiService.GetBattleSelectorViewModel()
        );

        IsBrowser = OperatingSystem.IsBrowser();
    }

    protected static DialogOptions GetDefaultDialogOptions()
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

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_battleStatsCts != null)
        {
            await _battleStatsCts.CancelAsync();
        }
    }

    protected virtual Task BattleSelectorOnValueChanged(BattleSearchRequest newValue)
    {
        AnalyticsService.TrackFormChange(newValue, DefaultAnalyticsParameters);

        BattleSearchRequest = newValue;

        return Task.CompletedTask;
    }
}
