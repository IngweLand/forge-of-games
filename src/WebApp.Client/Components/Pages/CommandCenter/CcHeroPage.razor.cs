using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Core.Helpers;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public partial class CcHeroPage : CommandCenterPageBase, IAsyncDisposable
{
    private CancellationTokenSource _cts = new();
    private HeroProfileViewModel? _heroProfileViewModel;
    private IReadOnlyCollection<IconLabelItemViewModel>? _progressionCost;

    private IReadOnlyCollection<UnitBattleViewModel>? _unitBattles;

    [Inject]
    private IBattleSearchRequestFactory BattleSearchRequestFactory { get; set; }

    [Inject]
    private ICcHeroesPlaygroundUiService PlaygroundUiService { get; set; }

    [Inject]
    private ICcProfileUiService ProfileUiService { get; set; }

    [Inject]
    private IStatsHubUiService StatsHubUiService { get; set; }

    [Inject]
    public ITreasureHuntUiService TreasureHuntUiService { get; set; }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }

    protected override async Task HandleOnParametersSetAsync()
    {
        await base.HandleOnParametersSetAsync();

        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(ProfileId))
        {
            _heroProfileViewModel = await PlaygroundUiService.GetHeroProfileAsync(HeroProfileId);
        }
        else
        {
            _heroProfileViewModel = await ProfileUiService.GetHeroProfileAsync(ProfileId, HeroProfileId);
        }

        if (_heroProfileViewModel != null)
        {
            await _cts.CancelAsync();
            _cts.Dispose();
            _cts = new CancellationTokenSource();

            var unitBattlesTask = StatsHubUiService.GetUnitBattlesAsync(_heroProfileViewModel.HeroUnitId, _cts.Token);
            var encounterMapTask = TreasureHuntUiService.GetBattleEncounterToIndexMapAsync();
            await Task.WhenAll(unitBattlesTask, encounterMapTask);
            _unitBattles = unitBattlesTask.Result;
        }
    }

    private void OnProgressionTargetLevelChanged(HeroProgressionCostRequest request)
    {
        _progressionCost = CommandCenterUiService.CalculateHeroProgressionCost(request);
    }

    private void UpdateProfile(HeroProfileStatsUpdateRequest request)
    {
        if (string.IsNullOrWhiteSpace(ProfileId))
        {
            _heroProfileViewModel = PlaygroundUiService.UpdateHeroProfile(request);
        }
        else
        {
            _heroProfileViewModel = ProfileUiService.UpdateHeroProfile(request);
        }
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        await _cts.CancelAsync();
        _cts.Dispose();
    }

    private async Task OpenBattle(UnitBattleViewModel unitBattle)
    {
        var query = BattleSearchRequestFactory.CreateQueryParams(unitBattle.BattleDefinitionId, unitBattle.Difficulty,
            unitBattle.BattleType, [unitBattle.UnitId, unitBattle.UnitId],
            await TreasureHuntUiService.GetBattleEncounterToIndexMapAsync());

        NavigationManager.NavigateTo(
            NavigationManager.GetUriWithQueryParameters(FogUrlBuilder.PageRoutes.BATTLE_LOG_PATH, query), false);
    }
}
