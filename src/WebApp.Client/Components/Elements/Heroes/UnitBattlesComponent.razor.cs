using Ingweland.Fog.Application.Client.Core.Localization;
using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
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
    private BattleType _selectedBattleType;
    private IReadOnlyCollection<UnitBattleViewModel>? _unitBattles;
    private IReadOnlyCollection<UnitBattleTypeViewModel> _unitBattleTypes = [];

    [Parameter]
    public IReadOnlyDictionary<string, object> AdditionalAnalyticsParams { get; set; } =
        new Dictionary<string, object>();

    [Inject]
    private IAnalyticsService AnalyticsService { get; set; }

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
    private IBattleUiService BattleUiService { get; set; }

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

    private void TrackEvent(string eventName, Dictionary<string, object> eventParams)
    {
        var allParameters = new Dictionary<string, object>(AdditionalAnalyticsParams);
        foreach (var eventParam in eventParams)
        {
            allParameters[eventParam.Key] = eventParam.Value;
        }

        allParameters[AnalyticsParams.UNIT_ID] = UnitId;
        allParameters[AnalyticsParams.SOURCE] = AnalyticsParams.Values.Sources.UNIT_BATTLES_COMPONENTS;

        _ = AnalyticsService.TrackEvent(eventName, allParameters);
    }

    protected override async Task OnInitializedAsync()
    {
        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        _unitBattleTypes = BattleUiService.GetUnitBattleTypes();
        _selectedBattleType = _unitBattleTypes.First().BattleType;
        _ = await TreasureHuntUiService.GetBattleEncounterToIndexMapAsync();
    }

    private async Task OnExpanded(bool expanded)
    {
        if (expanded)
        {
            await GetBattles(_selectedBattleType);
        }
    }

    private Task OnBattleTypeChanged(BattleType battleType)
    {
        TrackEvent(AnalyticsEvents.SELECT_HERO_BATTLE_TYPE, new Dictionary<string, object>
            {{AnalyticsParams.BATTLE_TYPE, battleType.ToString()}});

        return GetBattles(battleType);
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
        _unitBattles = [];
        StateHasChanged();
        try
        {
            _unitBattles = await BattleUiService.GetUnitBattlesAsync(UnitId, _selectedBattleType, _cts.Token);
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
            unitBattle.BattleType, [unitBattle.UnitId], await TreasureHuntUiService.GetBattleEncounterToIndexMapAsync());

        TrackEvent(AnalyticsEvents.NAVIGATE_HERO_BATTLE, new Dictionary<string, object>
        {
            {AnalyticsParams.BATTLE_DEFINITION_ID, unitBattle.BattleDefinitionId},
        });

        NavigationManager.NavigateTo(
            NavigationManager.GetUriWithQueryParameters(FogUrlBuilder.PageRoutes.BATTLE_LOG_PATH, query), false);
    }

    private void OnContributionPromptClick()
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.HELP_BATTLE_LOG_PATH);
    }
}
