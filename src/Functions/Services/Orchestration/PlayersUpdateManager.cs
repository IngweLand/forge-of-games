using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Providers;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Functions.Services.Interfaces;
using Ingweland.Fog.Functions.Services.Orchestration.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Errors;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services.Orchestration;

public class PlayersUpdateManager(
    IGameWorldsProvider gameWorldsProvider,
    IFogDbContext context,
    IInGameRawDataTableRepository inGameRawDataTableRepository,
    IInGameDataParsingService inGameDataParsingService,
    IFogPlayerService playerService,
    IFogAllianceService fogAllianceService,
    InGameRawDataTablePartitionKeyProvider inGameRawDataTablePartitionKeyProvider,
    IInGamePlayerService inGamePlayerService,
    DatabaseWarmUpService databaseWarmUpService,
    ILogger<PlayersUpdateManager> logger) : PlayersUpdateManagerBase(gameWorldsProvider, inGameRawDataTableRepository,
    inGameDataParsingService, playerService, fogAllianceService, inGameRawDataTablePartitionKeyProvider,
    inGamePlayerService, databaseWarmUpService, logger), IPlayersUpdateManager
{
    private const int BATCH_SIZE = 100;

    public async Task RunAsync(IReadOnlyCollection<PlayerKeyExtended> players)
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();
        Logger.LogDebug("Database warm-up completed");

        var removedPlayers = new HashSet<int>();
        var distinctPlayers = players.DistinctBy(x => x.Id).ToList();
        foreach (var player in distinctPlayers)
        {
            Logger.LogDebug("Processing player {@Player}", player);
            var gameWorld = GameWorldsProvider.GetGameWorlds().First(gw => gw.Id == player.WorldId);
            var delayTask = Task.Delay(500);
            var profile = await inGamePlayerService.FetchProfile(player);
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
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error upserting alliance/player {@Player}", player);
                }
            }
            else if (profile.HasError<PlayerNotFoundError>())
            {
                removedPlayers.Add(player.Id);
            }

            await delayTask;
        }

        if (removedPlayers.Count > 0)
        {
            await MarkMissingPlayers(removedPlayers);
        }

        logger.LogInformation("Processed {PlayerCount} players. Marked as inactive {InactivePlayerCount}",
            distinctPlayers.Count, removedPlayers.Count);
    }

    protected override async Task<List<Player>> GetPlayers(string gameWorldId)
    {
        Logger.LogDebug("Fetching players");

        return await GetInitQuery(gameWorldId)
            .Take(BATCH_SIZE)
            .ToListAsync();
    }

    private IQueryable<Player> GetInitQuery(string gameWorldId)
    {
        var profileUpdatedCutOff = DateTime.UtcNow.AddDays(-7).ToDateOnly();
        var lastSeenCutOff = DateTime.UtcNow.AddDays(-15);

        return context.Players
            .Where(x => x.Status == InGameEntityStatus.Active && x.WorldId == gameWorldId &&
                x.ProfileUpdatedAt < profileUpdatedCutOff &&
                (x.LastSeenOnline == null || x.LastSeenOnline > lastSeenCutOff))
            .OrderBy(x => x.ProfileUpdatedAt);
    }

    protected override Task<bool> HasMorePlayers(string gameWorldId)
    {
        return GetInitQuery(gameWorldId).AnyAsync();
    }
}
