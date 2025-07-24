using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Providers;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services.Orchestration.Abstractions;

public abstract class PlayersUpdateManagerBase(
    IGameWorldsProvider gameWorldsProvider,
    IInGameRawDataTableRepository inGameRawDataTableRepository,
    IInGameDataParsingService inGameDataParsingService,
    IPlayerStatusUpdaterService playerStatusUpdaterService,
    InGameRawDataTablePartitionKeyProvider inGameRawDataTablePartitionKeyProvider,
    IPlayerProfileService playerProfileService,
    DatabaseWarmUpService databaseWarmUpService,
    ILogger<PlayersUpdateManagerBase> logger) : OrchestratorBase(gameWorldsProvider, inGameRawDataTableRepository,
    inGameDataParsingService, inGameRawDataTablePartitionKeyProvider, logger)
{
    public async Task RunAsync()
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();
        logger.LogDebug("Database warm-up completed");

        var removedPlayers = new HashSet<int>();
        foreach (var gameWorld in GameWorldsProvider.GetGameWorlds())
        {
            var players = await GetPlayers(gameWorld.Id);
            logger.LogInformation("Retrieved {PlayerCount} players to process", players.Count);
            foreach (var player in players)
            {
                logger.LogDebug("Processing player {@Player}", player.Key);
                var delayTask = Task.Delay(1000);
                var profile = await playerProfileService.FetchProfile(player.Key);
                if (profile != null)
                {
                    try
                    {
                        await playerProfileService.UpsertPlayer(profile, gameWorld.Id);
                    }
                    catch
                    {
                        //ignore
                    }
                }
                else
                {
                    removedPlayers.Add(player.Id);
                }

                await delayTask;
            }
        }

        if (removedPlayers.Count > 0)
        {
            logger.LogInformation("Starting player status updater service with {PlayerCount} players to remove",
                removedPlayers.Count);
            await ExecuteSafeAsync(() => playerStatusUpdaterService.UpdateAsync(removedPlayers), "");
            logger.LogInformation("Completed player status updater service update");
        }
    }

    protected abstract Task<List<Player>> GetPlayers(string gameWorldId);
}
