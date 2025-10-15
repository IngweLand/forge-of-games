using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Caching.Interfaces;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Shared.Constants;

namespace Ingweland.Fog.Application.Client.Web.StatsHub;

public class StatsHubUiService : IStatsHubUiService
{
    private readonly Lazy<Task<IReadOnlyDictionary<string, AgeDto>>> _ages;
    private readonly IBattleService _battleService;
    private readonly IBattleViewModelFactory _battleViewModelFactory;
    private readonly ICommonService _commonService;
    private readonly IHohCoreDataCache _coreDataCache;
    private readonly IHohCoreDataViewModelsCache _coreDataViewModelsCache;
    private readonly IHeroProfileUiService _heroProfileUiService;
    private readonly IMapper _mapper;
    private readonly IStatsHubService _statsHubService;
    private readonly IStatsHubViewModelsFactory _statsHubViewModelsFactory;
    private readonly ITreasureHuntUiService _treasureHuntUiService;

    private TopStatsViewModel? _topStatsViewModel;

    public StatsHubUiService(IStatsHubService statsHubService,
        ICommonService commonService,
        IStatsHubViewModelsFactory statsHubViewModelsFactory,
        ITreasureHuntUiService treasureHuntUiService,
        IBattleService battleService,
        IHeroProfileUiService heroProfileUiService,
        IBattleViewModelFactory battleViewModelFactory,
        IHohCoreDataCache coreDataCache,
        IHohCoreDataViewModelsCache coreDataViewModelsCache,
        IMapper mapper)
    {
        _statsHubService = statsHubService;
        _commonService = commonService;
        _statsHubViewModelsFactory = statsHubViewModelsFactory;
        _treasureHuntUiService = treasureHuntUiService;
        _battleService = battleService;
        _heroProfileUiService = heroProfileUiService;
        _battleViewModelFactory = battleViewModelFactory;
        _coreDataCache = coreDataCache;
        _coreDataViewModelsCache = coreDataViewModelsCache;
        _mapper = mapper;

        _ages = new Lazy<Task<IReadOnlyDictionary<string, AgeDto>>>(GetAgesAsync);
    }

    public async Task<PlayerProfileViewModel?> GetPlayerProfileAsync(int playerId)
    {
        var player = await _statsHubService.GetPlayerProfileAsync(playerId);
        if (player == null)
        {
            return null;
        }

        var heroIds = player.Squads.Select(x => x.Hero.UnitId).ToHashSet();
        var heroes = await _coreDataCache.GetHeroes(heroIds);
        var treasureHuntDifficulties = await _coreDataViewModelsCache.GetBasicTreasureHuntDifficultiesAsync();
        var playerTreasureHuntDifficulty = player.TreasureHuntDifficulty != null
            ? treasureHuntDifficulties.FirstOrDefault(x => x.Difficulty == player.TreasureHuntDifficulty.Value)
            : null;
        var treasureHuntMaxPoints = playerTreasureHuntDifficulty != null
            ? _treasureHuntUiService.GetDifficultyMaxProgressPoints(playerTreasureHuntDifficulty.Difficulty)
            : 0;
        return _statsHubViewModelsFactory.CreatePlayerProfile(player, heroes, await _ages.Value,
            await _coreDataCache.GetBarracksByUnitMapAsync(),
            playerTreasureHuntDifficulty, treasureHuntMaxPoints, await _coreDataCache.GetRelicsAsync());
    }

    public async Task<PlayerViewModel?> GetPlayerAsync(int playerId, CancellationToken ct = default)
    {
        var player = await _statsHubService.GetPlayerAsync(playerId, ct);
        if (player == null)
        {
            return null;
        }

        var ages = await _ages.Value;
        return _mapper.Map<PlayerViewModel>(player, opt => { opt.Items[ResolutionContextKeys.AGES] = ages; });
    }

    public async Task<PaginatedList<PvpBattleViewModel>> GetPlayerBattlesAsync(PlayerViewModel player,
        int startIndex, int count,
        CancellationToken ct = default)
    {
        var result = await _statsHubService.GetPlayerBattlesAsync(player.Id, startIndex, count, ct);

        var heroIds = result.Items.SelectMany(b => b.WinnerUnits.Select(u => u.Hero!.UnitId))
            .Concat(result.Items.SelectMany(b => b.LoserUnits.Select(u => u.Hero!.UnitId)))
            .ToHashSet();
        var heroes = await _coreDataCache.GetHeroes(heroIds);
        var ages = await _ages.Value;
        var barracks = await _coreDataCache.GetBarracksByUnitMapAsync();
        var relics = await _coreDataCache.GetRelicsAsync();
        var newBattles = result.Items.Select(x =>
            _battleViewModelFactory.CreatePvpBattle(player, x, heroes, ages, barracks, relics)).ToList();
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
            topItems.MainWorldAlliances.Items, topItems.BetaWorldAlliances.Items, topItems.TopHeroes.Take(6).ToList(),
            await _ages.Value,
            await _heroProfileUiService.GetHeroes());

        return _topStatsViewModel;
    }

    public async Task<AllianceWithRankingsViewModel?> GetAllianceAsync(int allianceId)
    {
        var alliance = await _statsHubService.GetAllianceAsync(allianceId);
        if (alliance == null)
        {
            return null;
        }

        return _statsHubViewModelsFactory.CreateAlliance(alliance, await _ages.Value);
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

    public async Task<IReadOnlyCollection<BattleSummaryViewModel>> SearchBattles(
        BattleSearchRequest request, CancellationToken ct = default)
    {
        var result = await _battleService.SearchBattlesAsync(request, ct);

        return await _battleViewModelFactory.CreateBattleSummaryViewModels(result.Battles, request.BattleType);
    }

    private async Task<IReadOnlyDictionary<string, AgeDto>> GetAgesAsync()
    {
        return (await _commonService.GetAgesAsync()).ToDictionary(a => a.Id);
    }
}
