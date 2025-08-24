using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.WebApp.Client.Components.Elements.StatsHub;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using MudBlazor;
using Refit;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.StatsHub;

public partial class PlayerBattlesPage : StatsHubPageBase, IAsyncDisposable
{
    private const int DEBOUNCE_INTERVAL = 150;
    private static readonly ItemsProviderResult<PvpBattleViewModel> EmptyResult = new([], 0);
    private CancellationTokenSource? _battlesCts;
    private CancellationTokenSource? _battleStatsCts;

    private bool _isBrowser;

    private bool _isLoadingBattles = true;

    private PlayerViewModel? _player;
    private CancellationTokenSource? _playerCts;

    [Inject]
    public IBattleUiService BattleUiService { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }

    [Parameter]
    public required int PlayerId { get; set; }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _isBrowser = OperatingSystem.IsBrowser();
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (_player == null || _player.Id != PlayerId)
        {
            if (_playerCts != null)
            {
                await _playerCts.CancelAsync();
            }

            _playerCts = new CancellationTokenSource();
            IsInitialized = false;
            StateHasChanged();
            _player = await LoadWithPersistenceAsync(nameof(_player),
                () => StatsHubUiService.GetPlayerAsync(PlayerId, _playerCts.Token));
            IsInitialized = true;
        }
    }

    private async ValueTask<ItemsProviderResult<PvpBattleViewModel>> GetBattles(ItemsProviderRequest request)
    {
        if (_battlesCts != null)
        {
            await _battlesCts.CancelAsync();
        }

        _isLoadingBattles = true;
        StateHasChanged();

        if (_player == null)
        {
            _isLoadingBattles = false;
            return EmptyResult;
        }

        _battlesCts = new CancellationTokenSource();
        try
        {
            await Task.Delay(DEBOUNCE_INTERVAL, _battlesCts.Token);

            var result =
                await StatsHubUiService.GetPlayerBattlesAsync(_player, request.StartIndex, request.Count,
                    request.CancellationToken);
            return new ItemsProviderResult<PvpBattleViewModel>(result.Items, result.TotalCount);
        }
        catch (OperationCanceledException _)
        {
        }
        catch (ApiException apiEx) when (apiEx.InnerException is TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"Unexpected error: {ex}");
        }
        finally
        {
            _isLoadingBattles = false;
            StateHasChanged();
        }

        return EmptyResult;
    }

    private async Task OpenBattleStats(int? battleStatsId)
    {
        if (_battleStatsCts != null)
        {
            await _battleStatsCts.CancelAsync();
        }

        if (battleStatsId == null)
        {
            return;
        }

        _battleStatsCts = new CancellationTokenSource();
        var stats = await BattleUiService.GetBattleStatsAsync(battleStatsId.Value, _battleStatsCts.Token);

        var options = GetDefaultDialogOptions();

        var parameters = new DialogParameters<BattleStatsDialog> {{d => d.Stats, stats}};
        await DialogService.ShowAsync<BattleStatsDialog>(null, parameters, options);
    }

    private void OnPlayerClicked(int playerId)
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.Player(playerId));
    }

    private async Task OpenBattleSquadProfile(BattleSquadBasicViewModel squad)
    {
        var options = GetDefaultDialogOptions();

        var profile = await BattleUiService.CreateHeroProfile(squad);
        var parameters = new DialogParameters<ProfileSquadDialog> {{d => d.HeroProfile, profile}};
        await DialogService.ShowAsync<ProfileSquadDialog>(null, parameters, options);
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

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_playerCts != null)
        {
            await _playerCts.CancelAsync();
        }

        if (_battlesCts != null)
        {
            await _battlesCts.CancelAsync();
        }
    }
}
