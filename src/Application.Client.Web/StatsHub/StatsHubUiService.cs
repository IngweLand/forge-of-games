using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog;

namespace Ingweland.Fog.Application.Client.Web.StatsHub;

public class StatsHubUiService(
    IStatsHubService statsHubService,
    ICommonService commonService,
    IStatsHubViewModelsFactory statsHubViewModelsFactory,
    IBattleLogFactories battleLogFactories,
    ITreasureHuntUiService treasureHuntUiService,
    ICampaignUiService campaignUiService,
    IBattleService battleService,
    IUnitUiService unitUiService,
    IBattleStatsViewModelFactory battleStatsViewModelFactory) : IStatsHubUiService
{
    private readonly IDictionary<int, AllianceWithRankingsViewModel> _concreteAlliances =
        new Dictionary<int, AllianceWithRankingsViewModel>();

    private readonly IDictionary<int, PlayerWithRankingsViewModel> _concretePlayers =
        new Dictionary<int, PlayerWithRankingsViewModel>();

    private IReadOnlyDictionary<string, AgeDto>? _ages;
    private TopStatsViewModel? _topStatsViewModel;

    public async Task<PlayerWithRankingsViewModel?> GetPlayerAsync(int playerId)
    {
        if (_concretePlayers.TryGetValue(playerId, out var playerViewModel))
        {
            return playerViewModel;
        }

        await GetAgesAsync();
        var player = await statsHubService.GetPlayerAsync(playerId);
        if (player == null)
        {
            return null;
        }

        var newViewModel = statsHubViewModelsFactory.CreatePlayer(player, _ages!);
        _concretePlayers.Add(playerId, newViewModel);
        return newViewModel;
    }

    public async Task<TopStatsViewModel> GetTopStatsAsync()
    {
        if (_topStatsViewModel != null)
        {
            return _topStatsViewModel;
        }

        await GetAgesAsync();

        var topItems = await statsHubService.GetAllLeaderboardTopItemsAsync();

        _topStatsViewModel = statsHubViewModelsFactory.CreateTopStats(topItems.MainWorldPlayers.Items,
            topItems.BetaWorldPlayers.Items,
            topItems.MainWorldAlliances.Items, topItems.BetaWorldAlliances.Items, _ages!);

        return _topStatsViewModel;
    }

    public async Task<AllianceWithRankingsViewModel?> GetAllianceAsync(int allianceId)
    {
        if (_concreteAlliances.TryGetValue(allianceId, out var allianceViewModel))
        {
            return allianceViewModel;
        }

        var alliance = await statsHubService.GetAllianceAsync(allianceId);
        if (alliance == null)
        {
            return null;
        }

        await GetAgesAsync();
        var newViewModel = statsHubViewModelsFactory.CreateAlliance(alliance, _ages!);
        _concreteAlliances.Add(allianceId, newViewModel);
        return newViewModel;
    }

    public async Task<PaginatedList<AllianceViewModel>> GetAllianceStatsAsync(string worldId, int pageNumber = 1,
        string? allianceName = null,
        CancellationToken ct = default)
    {
        var result =
            await statsHubService.GetAlliancesAsync(worldId, pageNumber, name: allianceName,
                ct: ct);
        return statsHubViewModelsFactory.CreateAlliances(result);
    }

    public async Task<PaginatedList<PlayerViewModel>> GetPlayerStatsAsync(string worldId, int pageNumber = 1,
        string? playerName = null, CancellationToken ct = default)
    {
        await GetAgesAsync();
        var result =
            await statsHubService.GetPlayersAsync(worldId, pageNumber, name: playerName, ct: ct);
        return statsHubViewModelsFactory.CreatePlayers(result, _ages!);
    }

    public async Task<BattleSelectorViewModel> GetBattleSelectorViewModel()
    {
        var campaignTask = campaignUiService.GetCampaignContinentsBasicDataAsync();
        var treasureHuntTask = treasureHuntUiService.GetDifficultiesAsync();
        var historicBattlesTask = campaignUiService.GetHistoricBattlesBasicDataAsync();
        var teslaStormTask = campaignUiService.GetTeslaStormRegionsBasicDataAsync();
        var heroesTask = unitUiService.GetHeroListAsync();
        await Task.WhenAll(campaignTask, treasureHuntTask, historicBattlesTask, heroesTask, teslaStormTask);
        return battleLogFactories.CreateBattleSelectorData(campaignTask.Result, treasureHuntTask.Result,
            historicBattlesTask.Result, teslaStormTask.Result,
            heroesTask.Result);
    }

    public async Task<IReadOnlyCollection<BattleSummaryViewModel>> SearchBattles(
        BattleSearchRequest request, CancellationToken ct = default)
    {
        var result = await battleService.SearchBattlesAsync(request, ct);
        var heroes = result.Heroes.ToDictionary(h => h.Unit.Id);
        return result.Battles.Select(src => statsHubViewModelsFactory.CreateBattleSummaryViewModel(src, heroes))
            .ToList();
    }

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

    public async Task<IReadOnlyCollection<UnitBattleViewModel>> GetUnitBattlesAsync(string unitId,
        CancellationToken ct = default)
    {
        var unitBattles = await battleService.GetUnitBattlesAsync(unitId, ct);
        var vms = statsHubViewModelsFactory.CreateUnitBattleViewModels(unitBattles)
            .OrderBy(x => x.BattleType.GetSortOrder());
        return vms.ToList();
    }

    private async Task GetAgesAsync()
    {
        if (_ages != null)
        {
            return;
        }

        _ages = (await commonService.GetAgesAsync()).ToDictionary(a => a.Id);
    }
}
