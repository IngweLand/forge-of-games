using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Providers;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Functions.Services;
using Ingweland.Fog.InnSdk.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Constants;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class PlayersUpdater(
    IGameWorldsProvider gameWorldsProvider,
    IFogDbContext context,
    IInnSdkClient innSdkClient,
    IPlayerRankingService playerRankingService,
    IPvpRankingService pvpRankingService,
    IInGameRawDataTableRepository inGameRawDataTableRepository,
    IInGameDataParsingService inGameDataParsingService,
    IPlayerService playerService,
    IPlayerAgeHistoryService playerAgeHistoryService,
    IPlayerNameHistoryService playerNameHistoryService,
    IAllianceService allianceService,
    IPlayerAllianceNameHistoryService playerAllianceNameHistoryService,
    IAllianceRankingService allianceRankingService,
    IAllianceNameHistoryService allianceNameHistoryService,
    IPlayerStatusUpdaterService playerStatusUpdaterService,
    InGameRawDataTablePartitionKeyProvider inGameRawDataTablePartitionKeyProvider,
    IAllianceMembersUpdaterService allianceMembersUpdaterService,
    IMapper mapper,
    DatabaseWarmUpService databaseWarmUpService,
    ILogger<PlayersUpdater> logger) : FunctionBase(gameWorldsProvider, inGameRawDataTableRepository,
    inGameDataParsingService, inGameRawDataTablePartitionKeyProvider, logger)
{
    private const int BatchSize = 100;

    [Function("PlayersUpdater")]
    public async Task Run([TimerTrigger("0 */15 1-5 * * *")] TimerInfo myTimer)
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();
        logger.LogDebug("Database warm-up completed");

        var playerAggregates = new List<PlayerAggregate>();
        var allianceAggregates = new List<AllianceAggregate>();
        var now = DateTime.UtcNow;
        var removedPlayers = new HashSet<int>();
        foreach (var gameWorld in GameWorldsProvider.GetGameWorlds())
        {
            var players = await GetPlayers(gameWorld.Id);
            logger.LogInformation("Retrieved {PlayerCount} players to process", players.Count);
            var profiles = new List<PlayerProfile>();
            foreach (var player in players)
            {
                logger.LogDebug("Processing player {PlayerId} from world {WorldId}", player.Id, player.WorldId);
                var profile = await FetchProfile(gameWorld, player);
                if (profile != null)
                {
                    profiles.Add(profile);
                }
                else
                {
                    removedPlayers.Add(player.Id);
                }

                await Task.Delay(1000);
            }

            foreach (var p in profiles)
            {
                if (p.Alliance != null)
                {
                    allianceAggregates.Add(mapper.Map<AllianceAggregate>(p.Alliance, opt =>
                    {
                        opt.Items[ResolutionContextKeys.WORLD_ID] = gameWorld.Id;
                        opt.Items[ResolutionContextKeys.DATE] = now;
                    }));
                }

                playerAggregates.Add(mapper.Map<PlayerAggregate>(p, opt =>
                {
                    opt.Items[ResolutionContextKeys.PLAYER_RANKING_TYPE] = PlayerRankingType.PowerPoints;
                    opt.Items[ResolutionContextKeys.WORLD_ID] = gameWorld.Id;
                    opt.Items[ResolutionContextKeys.DATE] = now;
                }));
            }
        }

        logger.LogInformation(
            "Total player aggregates: {count}, Total alliance aggregates: {count}, Total removed players: {count}",
            playerAggregates.Count, allianceAggregates.Count, removedPlayers.Count);

        logger.LogInformation("Starting alliance service update");
        await ExecuteSafeAsync(() => allianceService.AddAsync(allianceAggregates), "");
        logger.LogInformation("Completed alliance service update");

        logger.LogInformation("Starting player service update");
        await ExecuteSafeAsync(() => playerService.AddAsync(playerAggregates), "");
        logger.LogInformation("Completed player service update");

        logger.LogInformation("Starting alliance ranking service update");
        await ExecuteSafeAsync(() => allianceRankingService.AddOrUpdateRankingsAsync(allianceAggregates), "");
        logger.LogInformation("Completed alliance ranking service update");

        logger.LogInformation("Starting alliance name history service update");
        await ExecuteSafeAsync(() => allianceNameHistoryService.UpdateAsync(allianceAggregates), "");
        logger.LogInformation("Completed alliance name history service update");

        logger.LogInformation("Starting player ranking service update");
        await ExecuteSafeAsync(() => playerRankingService.AddOrUpdateRankingsAsync(playerAggregates), "");
        logger.LogInformation("Completed player ranking service update");

        logger.LogInformation("Starting pvp ranking service update");
        await ExecuteSafeAsync(() => pvpRankingService.AddOrUpdateRankingsAsync(playerAggregates), "");
        logger.LogInformation("Completed pvp ranking service update");

        logger.LogInformation("Starting player name history service update");
        await ExecuteSafeAsync(() => playerNameHistoryService.UpdateAsync(playerAggregates), "");
        logger.LogInformation("Completed player name history service update");

        logger.LogInformation("Starting player age history service update");
        await ExecuteSafeAsync(() => playerAgeHistoryService.UpdateAsync(playerAggregates), "");
        logger.LogInformation("Completed player age history service update");

        logger.LogInformation("Starting player alliance name history service update");
        await ExecuteSafeAsync(() => playerAllianceNameHistoryService.UpdateAsync(playerAggregates), "");
        logger.LogInformation("Completed player alliance name history service update");

        logger.LogInformation("Starting player status updater service update");
        await ExecuteSafeAsync(() => playerStatusUpdaterService.UpdateAsync(removedPlayers), "");
        logger.LogInformation("Completed player status updater service update");

        logger.LogInformation("Starting alliance members updater service update");
        await ExecuteSafeAsync(() => allianceMembersUpdaterService.UpdateAsync(playerAggregates), "");
        logger.LogInformation("Completed alliance members updater service update");
    }

    private async Task<List<Player>> GetPlayers(string gameWorldId)
    {
        logger.LogDebug("Fetching players");

        var today = DateTime.UtcNow.ToDateOnly();
        var yesterday = today.AddDays(-1);

        var players = await context.Players
            .Where(x => x.IsPresentInGame && x.WorldId == gameWorldId)
            .Where(x => (x.Rank == null || x.RankingPoints == null ||
                (x.AllianceName != null && x.CurrentAlliance == null)) && x.UpdatedAt < today)
            .Take(BatchSize)
            .ToListAsync();

        if (players.Count < BatchSize)
        {
            players.AddRange(await context.Players
                .Where(x => x.IsPresentInGame && x.WorldId == gameWorldId && x.UpdatedAt < yesterday)
                .OrderByDescending(x => x.RankingPoints)
                .Take(BatchSize)
                .ToListAsync());
        }

        return players.Take(BatchSize).ToList();
    }

    private async Task<PlayerProfile?> FetchProfile(GameWorldConfig gameWorldConfig, Player player)
    {
        try
        {
            return await innSdkClient.PlayerService.GetPlayerProfileAsync(gameWorldConfig, player.InGamePlayerId);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting player's profile {PlayerId} from world {WorldId}: {ErrorMessage}",
                player.Id, player.WorldId, e.Message);
            return null;
        }
    }
}
