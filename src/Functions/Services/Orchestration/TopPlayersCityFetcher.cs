using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.PlayerCity.Abstractions;
using Ingweland.Fog.Functions.Services.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services.Orchestration;

public class TopPlayersCityFetcher(
    DatabaseWarmUpService databaseWarmUpService,
    IFogDbContext context,
    IPlayerCityService playerCityService,
    IPlayersUpdateManager playersUpdateManager,
    IMapper mapper,
    ILogger<PlayerCityFetcher> logger)
    : PlayerCityFetcher(databaseWarmUpService, context, playerCityService, playersUpdateManager, mapper, logger),
        ITopPlayersCityFetcher
{
    private const int TOP_RANK_LIMIT = 500;

    protected override async Task<List<PlayerKeyExtended>> GetPlayers()
    {
        var players = await GetInitQuery()
            .Take(BATCH_SIZE)
            .ProjectTo<PlayerKeyExtended>(Mapper.ConfigurationProvider)
            .ToListAsync();
        players = await FilterOutWithExistingCities(players);

        Logger.LogInformation("Selected {PlayerCount} players", players.Count);

        return players;
    }

    protected override async Task<bool> HasMorePlayers()
    {
        var players = await GetInitQuery()
            .ProjectTo<PlayerKeyExtended>(Mapper.ConfigurationProvider)
            .ToListAsync();
        players = await FilterOutWithExistingCities(players);
        return players.Count > 0;
    }

    private async Task<List<PlayerKeyExtended>> FilterOutWithExistingCities(List<PlayerKeyExtended> players)
    {
        var existingCities = await GetExistingCities();
        return players.Where(x => !existingCities.Contains(x.Id)).ToList();
    }

    private Task<HashSet<int>> GetExistingCities()
    {
        var today = DateTime.UtcNow.ToDateOnly();
        return Context.PlayerCitySnapshots
            .Where(x => x.CityId == CityId.Capital && x.CollectedAt == today)
            .Select(x => x.PlayerId)
            .ToHashSetAsync();
    }

    private IQueryable<Player> GetInitQuery()
    {
        return Context.Players
            .Where(x => x.Status == InGameEntityStatus.Active)
            .OrderByDescending(x => x.RankingPoints)
            .Take(TOP_RANK_LIMIT);
    }
}
