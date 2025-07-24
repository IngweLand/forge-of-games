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

public class StatsHubUiService : IStatsHubUiService
{
    private readonly Lazy<Task<IReadOnlyDictionary<string, AgeDto>>> _ages;
    private readonly Lazy<Task<IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto>>> _barracks;
    private readonly IBattleLogFactories _battleLogFactories;
    private readonly IBattleService _battleService;
    private readonly IBattleStatsViewModelFactory _battleStatsViewModelFactory;
    private readonly ICampaignUiService _campaignUiService;
    private readonly ICityService _cityService;
    private readonly ICommonService _commonService;

    private readonly IDictionary<int, AllianceWithRankingsViewModel> _concreteAlliances =
        new Dictionary<int, AllianceWithRankingsViewModel>();

    private readonly IDictionary<int, PlayerProfileViewModel> _concretePlayerProfiles =
        new Dictionary<int, PlayerProfileViewModel>();

    private readonly IDictionary<int, PlayerViewModel> _concretePlayers =
        new Dictionary<int, PlayerViewModel>();

    private readonly IHohCoreDataCache _coreDataCache;
    private readonly IHeroProfileUiService _heroProfileUiService;
    private readonly IMapper _mapper;
    private readonly IStatsHubService _statsHubService;
    private readonly IStatsHubViewModelsFactory _statsHubViewModelsFactory;
    private readonly Lazy<Task<IReadOnlyCollection<TreasureHuntDifficultyBasicViewModel>>> _treasureHuntDifficulties;
    private readonly ITreasureHuntUiService _treasureHuntUiService;

    private readonly HashSet<BattleType> _unitBattleTypes =
    [
        BattleType.Pvp, BattleType.Campaign, BattleType.TreasureHunt, BattleType.HistoricBattle, BattleType.TeslaStorm,
    ];

    private TopStatsViewModel? _topStatsViewModel;

    public StatsHubUiService(IStatsHubService statsHubService,
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
        IMapper mapper)
    {
        _statsHubService = statsHubService;
        _commonService = commonService;
        _statsHubViewModelsFactory = statsHubViewModelsFactory;
        _battleLogFactories = battleLogFactories;
        _treasureHuntUiService = treasureHuntUiService;
        _campaignUiService = campaignUiService;
        _battleService = battleService;
        _heroProfileUiService = heroProfileUiService;
        _cityService = cityService;
        _battleStatsViewModelFactory = battleStatsViewModelFactory;
        _coreDataCache = coreDataCache;
        _mapper = mapper;

        _ages = new Lazy<Task<IReadOnlyDictionary<string, AgeDto>>>(GetAgesAsync);
        _barracks = new Lazy<Task<IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto>>>(GetBarracksAsync);
        _treasureHuntDifficulties = new Lazy<Task<IReadOnlyCollection<TreasureHuntDifficultyBasicViewModel>>>(
            treasureHuntUiService.GetDifficultiesAsync);
    }

    public async Task<PlayerProfileViewModel?> GetPlayerProfileAsync(int playerId)
    {
        if (_concretePlayerProfiles.TryGetValue(playerId, out var playerViewModel))
        {
            return playerViewModel;
        }

        var player = await _statsHubService.GetPlayerProfileAsync(playerId);
        if (player == null)
        {
            return null;
        }

        var heroIds = player.PvpBattles.SelectMany(b =>
                b.WinnerUnits.Select(u => u.Hero!.UnitId).Concat(b.LoserUnits.Select(u => u.Hero!.UnitId)))
            .ToHashSet();
        var heroes = await GetAllBattleHeroes(heroIds);
        var treasureHuntDifficulties = await _treasureHuntDifficulties.Value;
        var playerTreasureHuntDifficulty = player.TreasureHuntDifficulty != null
            ? treasureHuntDifficulties.FirstOrDefault(x => x.Difficulty == player.TreasureHuntDifficulty.Value)
            : null;
        var treasureHuntMaxPoints = playerTreasureHuntDifficulty != null
            ? _treasureHuntUiService.GetDifficultyMaxProgressPoints(playerTreasureHuntDifficulty.Difficulty)
            : 0;
        var newViewModel =
            _statsHubViewModelsFactory.CreatePlayerProfile(player, heroes, await _ages.Value, await _barracks.Value,
                playerTreasureHuntDifficulty, treasureHuntMaxPoints);
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
        var player = await _statsHubService.GetPlayerAsync(playerId, ct);
        if (player == null)
        {
            return null;
        }

        var newViewModel =
            _mapper.Map<PlayerViewModel>(player, opt => { opt.Items[ResolutionContextKeys.AGES] = _ages; });
        ;
        _concretePlayers.Add(playerId, newViewModel);
        return newViewModel;
    }

    public async Task<PaginatedList<PvpBattleViewModel>> GetPlayerBattlesAsync(PlayerViewModel player,
        int startIndex, int count,
        CancellationToken ct = default)
    {
        var result = await _statsHubService.GetPlayerBattlesAsync(player.Id, startIndex, count, ct);

        var heroIds = result.Items.SelectMany(b => b.WinnerUnits.Select(u => u.Hero!.UnitId))
            .Concat(result.Items.SelectMany(b => b.LoserUnits.Select(u => u.Hero!.UnitId)))
            .ToHashSet();
        var heroes = await GetAllBattleHeroes(heroIds);
        var ages = await _ages.Value;
        var barracks = await _barracks.Value;
        var newBattles = result.Items.Select(x =>
            _statsHubViewModelsFactory.CreatePvpBattle(player, x, heroes, ages, barracks)).ToList();
        return new PaginatedList<PvpBattleViewModel>(newBattles, result.StartIndex, result.TotalCount);
    }

    public async Task<TopStatsViewModel> GetTopStatsAsync()
    {
        if (_topStatsViewModel != null)
        {
            return _topStatsViewModel;
        }

        var topItems = await _statsHubService.GetAllLeaderboardTopItemsAsync();

        _topStatsViewModel = _statsHubViewModelsFactory.CreateTopStats(topItems.MainWorldPlayers.Items,
            topItems.BetaWorldPlayers.Items,
            topItems.MainWorldAlliances.Items, topItems.BetaWorldAlliances.Items, topItems.TopHeroes, await _ages.Value,
            await _heroProfileUiService.GetHeroes());

        return _topStatsViewModel;
    }

    public async Task<AllianceWithRankingsViewModel?> GetAllianceAsync(int allianceId)
    {
        if (_concreteAlliances.TryGetValue(allianceId, out var allianceViewModel))
        {
            return allianceViewModel;
        }

        var alliance = await _statsHubService.GetAllianceAsync(allianceId);
        if (alliance == null)
        {
            return null;
        }

        var newViewModel = _statsHubViewModelsFactory.CreateAlliance(alliance, await _ages.Value);
        _concreteAlliances.Add(allianceId, newViewModel);
        return newViewModel;
    }

    public async Task<PaginatedList<AllianceViewModel>> GetAllianceStatsAsync(string worldId, int startIndex,
        int pageSize,
        string? allianceName = null, CancellationToken ct = default)
    {
        var result =
            await _statsHubService.GetAlliancesAsync(worldId, startIndex, pageSize, allianceName, ct);
        return _statsHubViewModelsFactory.CreateAlliances(result);
    }

    public async Task<PaginatedList<PlayerViewModel>> GetPlayerStatsAsync(string worldId, int startIndex, int pageSize,
        string? playerName = null, CancellationToken ct = default)
    {
        var result =
            await _statsHubService.GetPlayersAsync(worldId, startIndex, pageSize, playerName, ct);
        return _statsHubViewModelsFactory.CreatePlayers(result, await _ages.Value);
    }

    public async Task<BattleSelectorViewModel> GetBattleSelectorViewModel()
    {
        var campaignTask = _campaignUiService.GetCampaignContinentsBasicDataAsync();
        var treasureHuntTask = _treasureHuntDifficulties.Value;
        var historicBattlesTask = _campaignUiService.GetHistoricBattlesBasicDataAsync();
        var teslaStormTask = _campaignUiService.GetTeslaStormRegionsBasicDataAsync();
        var heroesTask = _heroProfileUiService.GetHeroes();
        await Task.WhenAll(campaignTask, treasureHuntTask, historicBattlesTask, heroesTask, teslaStormTask);
        return _battleLogFactories.CreateBattleSelectorData(campaignTask.Result, treasureHuntTask.Result,
            historicBattlesTask.Result, teslaStormTask.Result,
            heroesTask.Result);
    }

    public IReadOnlyCollection<UnitBattleTypeViewModel> GetUnitBattleTypes()
    {
        return _statsHubViewModelsFactory.CreateUnitBattleTypes(_unitBattleTypes.OrderBy(x => x.GetSortOrder()));
    }

    public async Task<IReadOnlyCollection<BattleSummaryViewModel>> SearchBattles(
        BattleSearchRequest request, CancellationToken ct = default)
    {
        var barracks = await _barracks.Value;
        var result = await _battleService.SearchBattlesAsync(request, ct);

        var heroIds =
            result.Battles.SelectMany(src => src.PlayerSquads.Select(s => s.Hero?.UnitId).Where(s => s != null));
        if (request.BattleType == BattleType.Pvp)
        {
            heroIds = heroIds.Concat(result.Battles.SelectMany(src =>
                src.EnemySquads.Select(s => s.Hero?.UnitId).Where(s => s != null)));
        }

        var heroes = (await GetAllBattleHeroes(heroIds.ToHashSet()!)).ToDictionary(h => h.Unit.Id);

        return result.Battles
            .Select(src => _statsHubViewModelsFactory.CreateBattleSummaryViewModel(src, heroes, barracks))
            .ToList();
    }

    public async Task<BattleStatsViewModel> GetBattleStatsAsync(
        int battleStatsId, CancellationToken ct = default)
    {
        var result = await _battleService.GetBattleStatsAsync(battleStatsId, ct);
        if (result == null)
        {
            return BattleStatsViewModel.Blank;
        }

        return _battleStatsViewModelFactory.Create(result);
    }

    public async Task<IReadOnlyCollection<UnitBattleViewModel>> GetUnitBattlesAsync(string unitId,
        BattleType battleType, CancellationToken ct = default)
    {
        var unitBattles = await _battleService.GetUnitBattlesAsync(unitId, battleType, ct);
        var vms = _statsHubViewModelsFactory.CreateUnitBattleViewModels(unitBattles)
            .OrderBy(x => x.BattleType.GetSortOrder());
        return vms.ToList();
    }

    private async Task<List<HeroDto>> GetAllBattleHeroes(HashSet<string> heroIds)
    {
        var heroTasks = heroIds.Select(_coreDataCache.GetHeroAsync);
        return (await Task.WhenAll(heroTasks)).Where(x => x != null).ToList()!;
    }

    private async Task<IReadOnlyDictionary<string, AgeDto>> GetAgesAsync()
    {
        return (await _commonService.GetAgesAsync()).ToDictionary(a => a.Id);
    }

    private async Task<IReadOnlyDictionary<(string unitId, int unitLevel), BuildingDto>> GetBarracksAsync()
    {
        var barracks = await _cityService.GetAllBarracks();
        var result = new Dictionary<(string unitId, int unitLevel), BuildingDto>();
        foreach (var b in barracks)
        {
            var component = b.Components.OfType<BuildingUnitProviderComponent>().FirstOrDefault();
            if (component == null)
            {
                continue;
            }

            result.Add((component.BuildingUnit.Unit.Id, component.BuildingUnit.Level), b);
        }

        return result;
    }
}
