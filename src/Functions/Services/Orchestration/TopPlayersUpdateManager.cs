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

public class TopPlayersUpdateManager(
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
    ILogger<TopPlayersUpdateManager> logger) : PlayersUpdateManagerBase(gameWorldsProvider, innSdkClient,
    playerRankingService, pvpRankingService, inGameRawDataTableRepository, inGameDataParsingService,
    playerService, playerAgeHistoryService, playerNameHistoryService, allianceService, playerAllianceNameHistoryService,
    allianceRankingService, allianceNameHistoryService, playerStatusUpdaterService,
    inGameRawDataTablePartitionKeyProvider, allianceMembersUpdaterService, playerSquadsUpdater, playerUpdater, mapper,
    databaseWarmUpService, logger), ITopPlayersUpdateManager
{
    private const int BATCH_SIZE = 100;
    private const int TOP_SKIP_COUNT = 100;
    private const int TOP_RANK_LIMIT = 500;

    protected override Task<List<Player>> GetPlayers(string gameWorldId)
    {
        logger.LogDebug("Fetching players");

        var today = DateTime.UtcNow.ToDateOnly();

        return context.Players
            .Where(x => x.IsPresentInGame && x.WorldId == gameWorldId)
            .OrderByDescending(x => x.RankingPoints)
            .Take(TOP_RANK_LIMIT)
            .Skip(TOP_SKIP_COUNT)
            .Where(x => x.UpdatedAt < today)
            .Take(BATCH_SIZE)
            .ToListAsync();
    }
}
