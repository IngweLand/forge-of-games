using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Caching.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Constants;

namespace Ingweland.Fog.Application.Client.Web.StatsHub;

public class StatsHubUiService(
    IStatsHubService statsHubService,
    ICommonService commonService,
    IStatsHubViewModelsFactory statsHubViewModelsFactory,
    IBattleLogFactories battleLogFactories,
    ITreasureHuntUiService treasureHuntUiService,
    ICampaignUiService campaignUiService,
    IBattleService battleService,
    IHeroProfileUiService heroProfileUiService,
    ICityService cityService,
    IBattleStatsViewModelFactory battleStatsViewModelFactory,
    IHohCoreDataCache coreDataCache,
    IMapper mapper) : IStatsHubUiService
{
    private readonly IDictionary<int, AllianceWithRankingsViewModel> _concreteAlliances =
        new Dictionary<int, AllianceWithRankingsViewModel>();

    private readonly IDictionary<int, PlayerProfileViewModel> _concretePlayerProfiles =
        new Dictionary<int, PlayerProfileViewModel>();

    private readonly IDictionary<int, PlayerViewModel> _concretePlayers =
        new Dictionary<int, PlayerViewModel>();

    private readonly HashSet<BattleType> _unitBattleTypes =
    [
        BattleType.Pvp, BattleType.Campaign, BattleType.TreasureHunt, BattleType.HistoricBattle, BattleType.TeslaStorm,
    ];

    private IReadOnlyDictionary<string, AgeDto>? _ages;

    private IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto>? _barracks;
    private TopStatsViewModel? _topStatsViewModel;

    public async Task<PlayerProfileViewModel?> GetPlayerProfileAsync(int playerId)
    {
        if (_concretePlayerProfiles.TryGetValue(playerId, out var playerViewModel))
        {
            return playerViewModel;
        }

        await GetAgesAsync();
        await GetBarracksAsync();
        var player = await statsHubService.GetPlayerProfileAsync(playerId);
        if (player == null)
        {
            return null;
        }

        var heroIds = player.PvpBattles.SelectMany(b =>
                b.WinnerUnits.Select(u => u.Hero!.UnitId).Concat(b.LoserUnits.Select(u => u.Hero!.UnitId)))
            .ToHashSet();
        var heroes = await GetAllBattleHeroes(heroIds);
        var newViewModel = statsHubViewModelsFactory.CreatePlayerProfile(player, heroes, _ages!, _barracks!);
        _concretePlayerProfiles.Add(playerId, newViewModel);
        return newViewModel;
    }

    public async Task<PlayerViewModel?> GetPlayerAsync(int playerId, CancellationToken ct = default)
    {
        if (_concretePlayers.TryGetValue(playerId, out var playerViewModel))
        {
            return playerViewModel;
        }

        await GetAgesAsync();
        var player = await statsHubService.GetPlayerAsync(playerId, ct);
        if (player == null)
        {
            return null;
        }

        var newViewModel =
            mapper.Map<PlayerViewModel>(player, opt => { opt.Items[ResolutionContextKeys.AGES] = _ages; });
        ;
        _concretePlayers.Add(playerId, newViewModel);
        return newViewModel;
    }

    public async Task<PaginatedList<PvpBattleViewModel>> GetPlayerBattlesAsync(PlayerViewModel player,
        int startIndex, int count,
        CancellationToken ct = default)
    {
        await GetAgesAsync();
        await GetBarracksAsync();
        var result = await statsHubService.GetPlayerBattlesAsync(player.Id, startIndex, count, ct);

        var heroIds = result.Items.SelectMany(b => b.WinnerUnits.Select(u => u.Hero!.UnitId))
            .Concat(result.Items.SelectMany(b => b.LoserUnits.Select(u => u.Hero!.UnitId)))
            .ToHashSet();
        var heroes = await GetAllBattleHeroes(heroIds);
        var newBattles = result.Items.Select(x =>
            statsHubViewModelsFactory.CreatePvpBattle(player, x, heroes, _ages!, _barracks!)).ToList();
        return new PaginatedList<PvpBattleViewModel>(newBattles, result.StartIndex, result.TotalCount);
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
            topItems.MainWorldAlliances.Items, topItems.BetaWorldAlliances.Items, topItems.TopHeroes, _ages!,
            await heroProfileUiService.GetHeroes());

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

    public async Task<PaginatedList<AllianceViewModel>> GetAllianceStatsAsync(string worldId, int startIndex,
        int pageSize,
        string? allianceName = null, CancellationToken ct = default)
    {
        var result =
            await statsHubService.GetAlliancesAsync(worldId, startIndex, pageSize, allianceName, ct);
        return statsHubViewModelsFactory.CreateAlliances(result);
    }

    public async Task<PaginatedList<PlayerViewModel>> GetPlayerStatsAsync(string worldId, int startIndex, int pageSize,
        string? playerName = null, CancellationToken ct = default)
    {
        await GetAgesAsync();
        var result =
            await statsHubService.GetPlayersAsync(worldId, startIndex, pageSize, playerName, ct);
        return statsHubViewModelsFactory.CreatePlayers(result, _ages!);
    }

    public async Task<BattleSelectorViewModel> GetBattleSelectorViewModel()
    {
        var campaignTask = campaignUiService.GetCampaignContinentsBasicDataAsync();
        var treasureHuntTask = treasureHuntUiService.GetDifficultiesAsync();
        var historicBattlesTask = campaignUiService.GetHistoricBattlesBasicDataAsync();
        var teslaStormTask = campaignUiService.GetTeslaStormRegionsBasicDataAsync();
        var heroesTask = heroProfileUiService.GetHeroes();
        await Task.WhenAll(campaignTask, treasureHuntTask, historicBattlesTask, heroesTask, teslaStormTask);
        return battleLogFactories.CreateBattleSelectorData(campaignTask.Result, treasureHuntTask.Result,
            historicBattlesTask.Result, teslaStormTask.Result,
            heroesTask.Result);
    }

    public IReadOnlyCollection<UnitBattleTypeViewModel> GetUnitBattleTypes()
    {
        return statsHubViewModelsFactory.CreateUnitBattleTypes(_unitBattleTypes.OrderBy(x => x.GetSortOrder()));
    }

    public async Task<IReadOnlyCollection<BattleSummaryViewModel>> SearchBattles(
        BattleSearchRequest request, CancellationToken ct = default)
    {
        await GetBarracksAsync();

        var result = await battleService.SearchBattlesAsync(request, ct);
        var heroes = result.Heroes.ToDictionary(h => h.Unit.Id);
        return result.Battles
            .Select(src => statsHubViewModelsFactory.CreateBattleSummaryViewModel(src, heroes, _barracks!))
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
        BattleType battleType, CancellationToken ct = default)
    {
        var unitBattles = await battleService.GetUnitBattlesAsync(unitId, battleType, ct);
        var vms = statsHubViewModelsFactory.CreateUnitBattleViewModels(unitBattles)
            .OrderBy(x => x.BattleType.GetSortOrder());
        return vms.ToList();
    }

    private async Task<List<HeroDto>> GetAllBattleHeroes(HashSet<string> heroIds)
    {
        var heroTasks = heroIds.Select(coreDataCache.GetHeroAsync);
        return (await Task.WhenAll(heroTasks)).Where(x => x != null).ToList()!;
    }

    private async Task GetAgesAsync()
    {
        if (_ages != null)
        {
            return;
        }

        _ages = (await commonService.GetAgesAsync()).ToDictionary(a => a.Id);
    }

    private async Task GetBarracksAsync()
    {
        if (_barracks != null)
        {
            return;
        }

        var barracks = await cityService.GetAllBarracks();
        var temp = new Dictionary<(string unitId, int unitLevel), BuildingDto>();
        foreach (var b in barracks)
        {
            var component = b.Components.OfType<BuildingUnitProviderComponent>().FirstOrDefault();
            if (component == null)
            {
                continue;
            }

            temp.Add((component.BuildingUnit.Unit.Id, component.BuildingUnit.Level), b);
        }

        _barracks = temp;
    }
}
