using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services;

public interface IMissingPlayersVerificator
{
    Task RunAsync();
}

public class MissingPlayersVerificator(
    IGameWorldsProvider gameWorldsProvider,
    IFogDbContext context,
    IFogPlayerService playerService,
    IFogAllianceService fogAllianceService,
    IInGamePlayerService inGamePlayerService,
    DatabaseWarmUpService databaseWarmUpService,
    ILogger<MissingPlayersVerificator> logger) : IMissingPlayersVerificator
{
    private const int BATCH_SIZE = 100;

    public async Task RunAsync()
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();
        logger.LogDebug("Database warm-up completed");

        var i = 0;
        var totalActivatedPlayers = 0;
        while (true)
        {
            var players = await context.Players.Where(x => x.Status == InGameEntityStatus.Missing).OrderBy(x => x.Id)
                .Skip(i * BATCH_SIZE).Take(BATCH_SIZE).ToListAsync();
            if (players.Count == 0)
            {
                break;
            }

            var activatedPlayers = 0;
            foreach (var player in players)
            {
                logger.LogDebug("Processing player {@Player}", player.Key);
                var gameWorld = gameWorldsProvider.GetGameWorlds().First(gw => gw.Id == player.WorldId);
                var delayTask = Task.Delay(500);
                var profile = await inGamePlayerService.FetchProfile(player.Key);
                if (profile.IsSuccess)
                {
                    try
                    {
                        if (profile.Value.Alliance != null)
                        {
                            await fogAllianceService.UpsertAlliance(profile.Value.Alliance, player.WorldId,
                                DateTime.UtcNow);
                        }

                        await playerService.UpsertPlayerAsync(profile.Value, gameWorld.Id);
                        activatedPlayers++;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error upserting alliance/player {@Player}", player.Key);
                    }
                }

                await delayTask;
            }

            totalActivatedPlayers += activatedPlayers;
            logger.LogInformation("Processed {players} players. Activated players: {TotalActivatedPlayers}",
                players.Count, activatedPlayers);

            i++;
        }

        logger.LogInformation("Total activated players: {TotalActivatedPlayers}", totalActivatedPlayers);
    }
}
