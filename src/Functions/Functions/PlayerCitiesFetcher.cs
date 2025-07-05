using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class PlayerCitiesFetcher(
    DatabaseWarmUpService databaseWarmUpService,
    IFogDbContext context,
    IPlayerCityService playerCityService,
    ILogger<PlayerCitiesFetcher> logger)
{
    public const int BatchSize = 100;

    [Function("PlayerCitiesFetcher")]
    public async Task Run([TimerTrigger("0 0 */6 * * *")] TimerInfo myTimer)
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();
        logger.LogDebug("Database warm-up completed");

        var players = await GetPlayers();
        logger.LogInformation("Retrieved {PlayerCount} players to process", players.Count);

        var successCount = 0;
        foreach (var player in players)
        {
            logger.LogDebug("Processing player {PlayerId} from world {WorldId}", player.Id, player.WorldId);

            try
            {
                var success = await FetchCity(player);
                if (success)
                {
                    successCount++;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error processing player {PlayerId} from world {WorldId}: {ErrorMessage}",
                    player.Id, player.WorldId, e.Message);
            }

            await Task.Delay(1000);
        }

        logger.LogInformation(
            "PlayerCitiesFetcher completed. Processed {TotalPlayers} players, {SuccessCount} successful",
            players.Count, successCount);
    }

    private async Task<List<Player>> GetPlayers()
    {
        var today = DateTime.UtcNow.ToDateOnly();
        logger.LogDebug("Fetching players for date {Date}", today);

        var existingCities =
            await context.PlayerCitySnapshots
                .Where(x => x.CityId == CityId.Capital && x.CollectedAt == today)
                .Select(x => x.PlayerId)
                .ToHashSetAsync();

        logger.LogDebug("Found {ExistingCount} existing city snapshots for today", existingCities.Count);

        var runs = 0;
        List<Player> players = [];
        while (runs < 5 && players.Count < BatchSize)
        {
            var p = await context.Players.Where(x => x.RankingPoints != null && x.RankingPoints > 1000)
                .OrderBy(x => Guid.NewGuid())
                .Take(BatchSize)
                .ToListAsync();
            players.AddRange(p.Where(x => !existingCities.Contains(x.Id)));
            runs++;

            logger.LogDebug("Fetch attempt {RunNumber}: Retrieved {NewPlayers} new players",
                runs, players.Count);
        }

        var result = players.Take(BatchSize).ToList();
        logger.LogInformation("Final player selection complete. Selected {PlayerCount} players after {Runs} runs",
            result.Count, runs);

        return result;
    }

    private async Task<bool> FetchCity(Player player)
    {
        var fetchedCity = await playerCityService.FetchCityAsync(player.WorldId, player.InGamePlayerId);
        if (fetchedCity == null)
        {
            logger.LogWarning("Failed to fetch city for player {PlayerId} from world {WorldId}",
                player.Id, player.WorldId);
            return false;
        }

        var savedCity = await playerCityService.SaveCityAsync(player.Id, fetchedCity);
        if (savedCity == null)
        {
            logger.LogError("Failed to save city for player {PlayerId} from world {WorldId}",
                player.Id, player.WorldId);
            return false;
        }

        logger.LogDebug("Successfully processed city for player {PlayerId} from world {WorldId}",
            player.Id, player.WorldId);
        return true;
    }
}
