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

public class PlayerCityFetcher(
    DatabaseWarmUpService databaseWarmUpService,
    IFogDbContext context,
    IPlayerCityService playerCityService,
    IPlayersUpdateManager playersUpdateManager,
    ILogger<PlayerCityFetcher> logger) : IPlayerCityFetcher
{
    protected const int BATCH_SIZE = 100;
    protected IFogDbContext Context { get; } = context;
    protected ILogger<PlayerCityFetcher> Logger { get; } = logger;

    public async Task<bool> RunAsync()
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();
        Logger.LogDebug("Database warm-up completed");

        var players = await GetPlayers();
        Logger.LogInformation("Retrieved {PlayerCount} players to process", players.Count);

        var successCount = 0;
        var playersToVerify = new List<Player>();
        foreach (var player in players)
        {
            Logger.LogDebug("Processing player {PlayerId} from world {WorldId}", player.Id, player.WorldId);
            var delayTask = Task.Delay(500);
            try
            {
                var success = await FetchCity(player);
                if (success)
                {
                    successCount++;
                }
                else
                {
                    playersToVerify.Add(player);
                }
            }
            catch (Exception e)
            {
                playersToVerify.Add(player);
                Logger.LogError(e, "Error processing player {PlayerId} from world {WorldId}: {ErrorMessage}",
                    player.Id, player.WorldId, e.Message);
            }

            await delayTask;
        }

        Logger.LogInformation(
            "PlayerCitiesFetcher completed. Processed {TotalPlayers} players, {SuccessCount} successful",
            players.Count, successCount);
        
        if (playersToVerify.Count > 0)
        {
            await playersUpdateManager.RunAsync(playersToVerify);
        }

        return await HasMorePlayers();
    }

    protected virtual async Task<bool> HasMorePlayers()
    {
        var players = await GetPlayers();
        return players.Count > 0;
    }

    protected virtual async Task<List<Player>> GetPlayers()
    {
        var monthAgo = DateTime.UtcNow.ToDateOnly().AddMonths(-1);
        Logger.LogDebug("Fetching players starting from from {Date}", monthAgo);

        var existingCities =
            await Context.PlayerCitySnapshots
                .Where(x => x.CityId == CityId.Capital && x.CollectedAt > monthAgo)
                .Select(x => x.PlayerId)
                .ToHashSetAsync();

        Logger.LogDebug("Found {ExistingCount} existing city snapshots", existingCities.Count);

        var runs = 0;
        List<Player> players = [];
        while (runs < 10 && players.Count < BATCH_SIZE)
        {
            var p = await Context.Players
                .Where(x => x.Status == InGameEntityStatus.Active && x.RankingPoints > 1000)
                .OrderBy(x => Guid.NewGuid())
                .Take(BATCH_SIZE)
                .ToListAsync();
            players.AddRange(p.Where(x => !existingCities.Contains(x.Id)));
            runs++;

            Logger.LogDebug("Fetch attempt {RunNumber}: Retrieved {NewPlayers} new players",
                runs, players.Count);
        }

        var result = players.Take(BATCH_SIZE).ToList();
        Logger.LogInformation("Final player selection complete. Selected {PlayerCount} players after {Runs} runs",
            result.Count, runs);

        return result;
    }

    private async Task<bool> FetchCity(Player player)
    {
        var fetchedCity = await playerCityService.FetchCityAsync(player.WorldId, player.InGamePlayerId);
        if (fetchedCity == null)
        {
            Logger.LogWarning("Failed to fetch city for player {PlayerId} from world {WorldId}",
                player.Id, player.WorldId);
            return false;
        }

        var savedCity = await playerCityService.SaveCityAsync(player.Id, fetchedCity);
        if (savedCity == null)
        {
            Logger.LogError("Failed to save city for player {PlayerId} from world {WorldId}",
                player.Id, player.WorldId);
            return false;
        }

        Logger.LogDebug("Successfully processed city for player {PlayerId} from world {WorldId}",
            player.Id, player.WorldId);
        return true;
    }
}
