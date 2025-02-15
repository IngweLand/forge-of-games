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
    private IReadOnlyDictionary<string, AgeDto>? _ages;

    private IDictionary<PlayerKey, PlayerWithRankingsViewModel> _concretePlayers =
        new Dictionary<PlayerKey, PlayerWithRankingsViewModel>();

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
        var main = await statsHubService.GetPlayersAsync("un1");
        var beta = await statsHubService.GetPlayersAsync("zz1");

        return statsHubViewModelsFactory.CreateTopStats(main.Items, beta.Items, _ages!);
    }

    public async Task<PaginatedList<PlayerViewModel>> GetStatsAsync(string worldId, int pageNumber = 1,
        string? playerName = null, CancellationToken ct = default)
    {
        await GetAgesAsync();
        var result =
            await statsHubService.GetPlayersAsync(worldId, pageNumber: pageNumber, playerName: playerName, ct: ct);
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
