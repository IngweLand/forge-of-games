using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.PlayerCity.Abstractions;
using Ingweland.Fog.Functions.Services.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services.Orchestration;

public class TopPlayersCityFetcher(
    DatabaseWarmUpService databaseWarmUpService,
    IFogDbContext context,
    IPlayerCityService playerCityService,
    ILogger<PlayerCityFetcher> logger) : PlayerCityFetcher(databaseWarmUpService, context, playerCityService, logger),
    ITopPlayersCityFetcher
{
    protected override async Task<List<Player>> GetPlayers()
    {
        Logger.LogDebug("Fetching cities for top players");
        var today = DateTime.UtcNow.ToDateOnly();
        var existingCities =
            await Context.PlayerCitySnapshots
                .Where(x => x.CityId == CityId.Capital && x.CollectedAt == today)
                .Select(x => x.PlayerId)
                .ToHashSetAsync();

        Logger.LogDebug("Found {ExistingCount} existing city snapshots", existingCities.Count);

        var players = await Context.Players
            .Where(x => x.WorldId == "un1" && x.IsPresentInGame)
            .OrderByDescending(x => x.RankingPoints)
            .Take(BATCH_SIZE)
            .ToListAsync();
        players = players.Where(x => !existingCities.Contains(x.Id)).ToList();

        Logger.LogInformation("Selected {PlayerCount} players", players.Count);

        return players;
    }
}
