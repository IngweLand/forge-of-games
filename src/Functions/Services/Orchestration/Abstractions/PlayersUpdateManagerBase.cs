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
    IFogAllianceService fogAllianceService,
    InGameRawDataTablePartitionKeyProvider inGameRawDataTablePartitionKeyProvider,
    IInGamePlayerService inGamePlayerService,
    DatabaseWarmUpService databaseWarmUpService,
    ILogger<PlayersUpdateManagerBase> logger) : OrchestratorBase(gameWorldsProvider, inGameRawDataTableRepository,
    inGameDataParsingService, inGameRawDataTablePartitionKeyProvider, logger)
{
    public async Task<bool> RunAsync()
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();
        Logger.LogDebug("Database warm-up completed");

        var removedPlayers = new HashSet<int>();
        var hasMorePlayers = false;
        foreach (var gameWorld in GameWorldsProvider.GetGameWorlds())
        {
            var players = await GetPlayers(gameWorld.Id);
            hasMorePlayers = hasMorePlayers || await HasMorePlayers(gameWorld.Id);
            Logger.LogInformation("Retrieved {PlayerCount} players to process in {gw}", players.Count, gameWorld.Id);
            foreach (var player in players)
            {
                Logger.LogDebug("Processing player {@Player}", player.Key);
                var delayTask = Task.Delay(500);
                var profile = await inGamePlayerService.FetchProfile(player.Key);
                if (profile.IsSuccess)
                {
                    try
                    {
                        if (profile.Value.Alliance != null)
                        {
                            await fogAllianceService.UpsertAlliance(profile.Value.Alliance, player.WorldId, DateTime.UtcNow);
                        }
                        await playerService.UpsertPlayerAsync(profile.Value, gameWorld.Id);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "Error upserting alliance/player {@Player}", player.Key);
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
            await MarkMissingPlayers(removedPlayers);
        }

        return hasMorePlayers;
    }
    
    protected async Task MarkMissingPlayers(IReadOnlyCollection<int> playerIds)
    {
        Logger.LogInformation("Starting player status updater service with {PlayerCount} players to remove",
            playerIds.Count);
        await ExecuteSafeAsync(
            () => playerService.UpdateStatusAsync(playerIds, InGameEntityStatus.Missing, CancellationToken.None),
            "");
        Logger.LogInformation("Completed player status updater service update");
    }

    protected abstract Task<List<Player>> GetPlayers(string gameWorldId);
    protected abstract Task<bool> HasMorePlayers(string gameWorldId);
}
