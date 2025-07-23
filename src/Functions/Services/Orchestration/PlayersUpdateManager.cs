using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Providers;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Functions.Services.Interfaces;
using Ingweland.Fog.Functions.Services.Orchestration.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services.Orchestration;

public class PlayersUpdateManager(
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
    IPlayerSquadsUpdater playerSquadsUpdater,
    IPlayerUpdater playerUpdater,
    IMapper mapper,
    DatabaseWarmUpService databaseWarmUpService,
    ILogger<PlayersUpdateManager> logger) : PlayersUpdateManagerBase(gameWorldsProvider, innSdkClient,
    playerRankingService, pvpRankingService, inGameRawDataTableRepository, inGameDataParsingService,
    playerService, playerAgeHistoryService, playerNameHistoryService, allianceService, playerAllianceNameHistoryService,
    allianceRankingService, allianceNameHistoryService, playerStatusUpdaterService,
    inGameRawDataTablePartitionKeyProvider, allianceMembersUpdaterService, playerSquadsUpdater, playerUpdater, mapper,
    databaseWarmUpService, logger), IPlayersUpdateManager
{
    private const int BATCH_SIZE = 100;

    protected override async Task<List<Player>> GetPlayers(string gameWorldId)
    {
        logger.LogDebug("Fetching players");

        var today = DateTime.UtcNow.ToDateOnly();
        var yesterday = today.AddDays(-1);

        var players = await context.Players
            .Where(x => x.IsPresentInGame && x.WorldId == gameWorldId)
            .Where(x => (x.Rank == null || x.RankingPoints == null ||
                (x.AllianceName != null && x.CurrentAlliance == null)) && x.UpdatedAt < today)
            .Take(BATCH_SIZE)
            .ToListAsync();

        if (players.Count < BATCH_SIZE)
        {
            players.AddRange(await context.Players
                .Where(x => x.IsPresentInGame && x.WorldId == gameWorldId && x.UpdatedAt < yesterday)
                .OrderByDescending(x => x.RankingPoints)
                .Take(BATCH_SIZE)
                .ToListAsync());
        }

        return players.Take(BATCH_SIZE).ToList();
    }
}
