using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Providers;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.InnSdk.Hoh.Errors;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services.Orchestration.Abstractions;

public abstract class PlayersUpdateManagerBase(
    IGameWorldsProvider gameWorldsProvider,
    IInGameRawDataTableRepository inGameRawDataTableRepository,
    IInGameDataParsingService inGameDataParsingService,
    IFogPlayerService playerService,
    InGameRawDataTablePartitionKeyProvider inGameRawDataTablePartitionKeyProvider,
    IInGamePlayerService inGamePlayerService,
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
                var profile = await inGamePlayerService.FetchProfile(player.Key);
                if (profile.IsSuccess)
                {
                    try
                    {
                        await playerService.UpsertPlayer(profile.Value, gameWorld.Id);
                    }
                    catch
                    {
                        //ignore
                    }
                }
                else if (profile.HasError<PlayerNotFoundError>())
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
            await ExecuteSafeAsync(
                () => playerService.UpdateStatusAsync(removedPlayers, PlayerStatus.Missing, CancellationToken.None),
                "");
            logger.LogInformation("Completed player status updater service update");
        }
    }

    protected abstract Task<List<Player>> GetPlayers(string gameWorldId);
}
