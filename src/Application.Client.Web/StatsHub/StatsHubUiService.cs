using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.StatsHub;

public class StatsHubUiService(
    IStatsHubService statsHubService,
    ICommonService commonService,
    IStatsHubViewModelsFactory statsHubViewModelsFactory) : IStatsHubUiService
{
    private readonly IDictionary<AllianceKey, AllianceWithRankingsViewModel> _concreteAlliances =
        new Dictionary<AllianceKey, AllianceWithRankingsViewModel>();

    private readonly IDictionary<PlayerKey, PlayerWithRankingsViewModel> _concretePlayers =
        new Dictionary<PlayerKey, PlayerWithRankingsViewModel>();

    private IReadOnlyDictionary<string, AgeDto>? _ages;

    public async Task<PlayerWithRankingsViewModel?> GetPlayerAsync(string worldId, int playerId)
    {
        var key = new PlayerKey(worldId, playerId);
        if (_concretePlayers.TryGetValue(key, out var playerViewModel))
        {
            return playerViewModel;
        }

        await GetAgesAsync();
        var player = await statsHubService.GetPlayerAsync(key.WorldId, key.InGamePlayerId);
        if (player == null)
        {
            return null;
        }

        var newViewModel = statsHubViewModelsFactory.CreatePlayer(player, _ages!);
        _concretePlayers.Add(key, newViewModel);
        return newViewModel;
    }

    public async Task<TopStatsViewModel> GetTopStatsAsync()
    {
        await GetAgesAsync();
        var mainPlayersTask = statsHubService.GetPlayersAsync("un1");
        var betaPlayersTask = statsHubService.GetPlayersAsync("zz1");
        var mainAlliancesTask = statsHubService.GetAlliancesAsync("un1");
        var betaAlliancesTask = statsHubService.GetAlliancesAsync("zz1");

        await Task.WhenAll(mainPlayersTask, betaPlayersTask, mainAlliancesTask, betaAlliancesTask);

        return statsHubViewModelsFactory.CreateTopStats(mainPlayersTask.Result.Items, betaPlayersTask.Result.Items,
            mainAlliancesTask.Result.Items, betaAlliancesTask.Result.Items, _ages!);
    }

    public async Task<AllianceWithRankingsViewModel?> GetAllianceAsync(string worldId, int allianceId)
    {
        var key = new AllianceKey(worldId, allianceId);
        if (_concreteAlliances.TryGetValue(key, out var allianceViewModel))
        {
            return allianceViewModel;
        }

        var alliance = await statsHubService.GetAllianceAsync(key.WorldId, key.InGameAllianceId);
        if (alliance == null)
        {
            return null;
        }
        await GetAgesAsync();
        var newViewModel = statsHubViewModelsFactory.CreateAlliance(alliance, _ages!);
        _concreteAlliances.Add(key, newViewModel);
        return newViewModel;
    }

    public async Task<PaginatedList<AllianceViewModel>> GetAllianceStatsAsync(string worldId, int pageNumber = 1,
        string? allianceName = null,
        CancellationToken ct = default)
    {
        var result =
            await statsHubService.GetAlliancesAsync(worldId, pageNumber: pageNumber, name: allianceName,
                ct: ct);
        return statsHubViewModelsFactory.CreateAlliances(result);
    }

    public async Task<PaginatedList<PlayerViewModel>> GetPlayerStatsAsync(string worldId, int pageNumber = 1,
        string? playerName = null, CancellationToken ct = default)
    {
        await GetAgesAsync();
        var result =
            await statsHubService.GetPlayersAsync(worldId, pageNumber: pageNumber, name: playerName, ct: ct);
        return statsHubViewModelsFactory.CreatePlayers(result, _ages!);
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