using Ingweland.Fog.Application.Client.Web.Caching.Interfaces;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh;

public class BattleUiService(
    IBattleLogFactories battleLogFactories,
    ICampaignUiService campaignUiService,
    IBattleService battleService,
    IHeroProfileUiService heroProfileUiService,
    IBattleStatsViewModelFactory battleStatsViewModelFactory,
    IBattleViewModelFactory battleViewModelFactory,
    IHohCoreDataViewModelsCache coreDataViewModelsCache) : IBattleUiService
{
    private readonly HashSet<BattleType> _unitBattleTypes =
    [
        BattleType.Pvp, BattleType.Campaign, BattleType.TreasureHunt, BattleType.HistoricBattle, BattleType.TeslaStorm,
    ];

    public async Task<BattleStatsViewModel> GetBattleStatsAsync(
        int battleStatsId, CancellationToken ct = default)
    {
        var result = await battleService.GetBattleStatsAsync(battleStatsId, ct);
        if (result == null)
        {
            return BattleStatsViewModel.Blank;
        }

        return battleStatsViewModelFactory.Create(result);
    }

    public IReadOnlyCollection<UnitBattleTypeViewModel> GetUnitBattleTypes()
    {
        return battleViewModelFactory.CreateUnitBattleTypes(_unitBattleTypes.OrderBy(x => x.GetSortOrder()));
    }

    public async Task<BattleSelectorViewModel> GetBattleSelectorViewModel()
    {
        var campaignTask = campaignUiService.GetCampaignContinentsBasicDataAsync();
        var treasureHuntTask = coreDataViewModelsCache.GetBasicTreasureHuntDifficultiesAsync();
        var historicBattlesTask = campaignUiService.GetHistoricBattlesBasicDataAsync();
        var teslaStormTask = campaignUiService.GetTeslaStormRegionsBasicDataAsync();
        var heroesTask = heroProfileUiService.GetHeroes();
        await Task.WhenAll(campaignTask, treasureHuntTask, historicBattlesTask, heroesTask, teslaStormTask);
        return battleLogFactories.CreateBattleSelectorData(campaignTask.Result, treasureHuntTask.Result,
            historicBattlesTask.Result, teslaStormTask.Result,
            heroesTask.Result);
    }

    public async Task<IReadOnlyCollection<UnitBattleViewModel>> GetUnitBattlesAsync(string unitId,
        BattleType battleType, CancellationToken ct = default)
    {
        var unitBattles = await battleService.GetUnitBattlesAsync(unitId, battleType, ct);
        var vms = battleViewModelFactory.CreateUnitBattleViewModels(unitBattles)
            .OrderBy(x => x.BattleType.GetSortOrder());
        return vms.ToList();
    }

    public async Task<PaginatedList<BattleSummaryViewModel>> SearchBattles(
        UserBattleSearchRequest request, CancellationToken ct = default)
    {
        var result = await battleService.SearchBattlesAsync(request, ct);

        var battles =
            await battleViewModelFactory.CreateBattleSummaryViewModels(result.Items, request.BattleType);

        return new PaginatedList<BattleSummaryViewModel>(battles, result.StartIndex, result.TotalCount);
    }

    public async Task<BattleViewModel?> GetBattleAsync(int battleId, CancellationToken ct = default)
    {
        var result = await battleService.GetBattleAsync(battleId, ct);
        if (result == null)
        {
            return null;
        }

        return await battleViewModelFactory.CreateBattleViewModel(result);
    }
}
