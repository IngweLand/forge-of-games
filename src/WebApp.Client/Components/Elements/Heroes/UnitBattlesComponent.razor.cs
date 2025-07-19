using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Refit;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.Heroes;

public partial class UnitBattlesComponent : ComponentBase, IAsyncDisposable
{
    private CancellationTokenSource? _cts;
    private bool _isLoading = true;
    private string _lastUnitId = string.Empty;
    private BattleType _selectedBattleType;
    private IReadOnlyCollection<UnitBattleViewModel>? _unitBattles;
    private IReadOnlyCollection<UnitBattleTypeViewModel> _unitBattleTypes = [];

    [Inject]
    private IAssetUrlProvider AssetUrlProvider { get; set; }

    [Inject]
    private IBattleSearchRequestFactory BattleSearchRequestFactory { get; set; }

    [Inject]
    private IStringLocalizer<FogResource> Loc { get; set; }

    [Inject]
    public ILogger<UnitBattlesComponent> Logger { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Inject]
    private IStatsHubUiService StatsHubUiService { get; set; }

    [Inject]
    private ITreasureHuntUiService TreasureHuntUiService { get; set; }

    [Parameter]
    [EditorRequired]
    public required string UnitId { get; set; }

    public async ValueTask DisposeAsync()
    {
        if (_cts != null)
        {
            await _cts.CancelAsync();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        _unitBattleTypes = StatsHubUiService.GetUnitBattleTypes();
        _selectedBattleType = _unitBattleTypes.First().BattleType;
        _ = await TreasureHuntUiService.GetBattleEncounterToIndexMapAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (_lastUnitId == UnitId)
        {
            return;
        }

        _lastUnitId = UnitId;

        await GetBattles(_selectedBattleType);
    }

    private async Task GetBattles(BattleType battleType)
    {
        if (_cts != null)
        {
            await _cts.CancelAsync();
        }

        _cts = new CancellationTokenSource();
        _isLoading = true;
        _selectedBattleType = battleType;
        StateHasChanged();
        try
        {
            _unitBattles = await StatsHubUiService.GetUnitBattlesAsync(UnitId, _selectedBattleType, _cts.Token);
        }
        catch (OperationCanceledException _)
        {
            return;
        }
        catch (ApiException apiEx) when (apiEx.InnerException is TaskCanceledException)
        {
            return;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Unexpected error.");
        }

        _isLoading = false;
        StateHasChanged();
    }

    private async Task OpenBattle(UnitBattleViewModel unitBattle)
    {
        if (_isLoading)
        {
            return;
        }
        var query = BattleSearchRequestFactory.CreateQueryParams(unitBattle.BattleDefinitionId, unitBattle.Difficulty,
            unitBattle.BattleType, [unitBattle.UnitId, unitBattle.UnitId],
            await TreasureHuntUiService.GetBattleEncounterToIndexMapAsync());

        NavigationManager.NavigateTo(
            NavigationManager.GetUriWithQueryParameters(FogUrlBuilder.PageRoutes.BATTLE_LOG_PATH, query), false);
    }

    private void OnContributionPromptClick()
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.HELP_BATTLE_LOG_PATH);
    }
}
