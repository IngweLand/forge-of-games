using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.WebApp.Client.Components.Elements.StatsHub;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class PlayerPage : StatsHubPageBase, IAsyncDisposable
{
    private CancellationTokenSource _battleStatsCts = new();
    private bool _canShowChart;
    private PlayerWithRankingsViewModel? _player;

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

        if (OperatingSystem.IsBrowser())
        {
            _canShowChart = true;
            IsInitialized = true;
        }
    }

    protected override async Task OnParametersSetAsync()
    {
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
        await _battleStatsCts.CancelAsync();
        _battleStatsCts.Dispose();
    }

    private void SearchAlliance()
    {
        NavigationManager.NavigateTo(
            FogUrlBuilder.PageRoutes.SearchAlliance(_player!.Player.WorldId, _player.Player.AllianceName!));
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
        var options = GetDefaultDialogOptions();

        var parameters = new DialogParameters<BattleStatsDialog> {{d => d.Stats, stats}};
        await DialogService.ShowAsync<BattleStatsDialog>(null, parameters, options);
    }

    private void OnHeroClicked(string heroId)
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.HeroPlayground(heroId));
    }
}
