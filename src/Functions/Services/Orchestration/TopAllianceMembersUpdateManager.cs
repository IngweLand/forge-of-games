using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Providers;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Functions.Services.Interfaces;
using Ingweland.Fog.Functions.Services.Orchestration.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services.Orchestration;

public class TopAllianceMembersUpdateManager(
    IGameWorldsProvider gameWorldsProvider,
    IFogDbContext context,
    IInGameRawDataTableRepository inGameRawDataTableRepository,
    IInGameDataParsingService inGameDataParsingService,
    IFogPlayerService playerService,
    InGameRawDataTablePartitionKeyProvider inGameRawDataTablePartitionKeyProvider,
    IInGamePlayerService inGamePlayerService,
    DatabaseWarmUpService databaseWarmUpService,
    ILogger<PlayersUpdateManager> logger) : PlayersUpdateManagerBase(gameWorldsProvider, inGameRawDataTableRepository,
    inGameDataParsingService, playerService, inGameRawDataTablePartitionKeyProvider, inGamePlayerService,
    databaseWarmUpService, logger), ITopAllianceMembersUpdateManager
{
    private const int BATCH_SIZE = 100;
    private const int TOP_RANK_LIMIT = 15;

    protected override async Task<List<Player>> GetPlayers(string gameWorldId)
    {
        logger.LogDebug("Fetching players");

        var today = DateTime.UtcNow.ToDateOnly();

        var alliances = await context.Alliances
            .Include(x => x.Members)
            .Where(x => x.WorldId == gameWorldId)
            .OrderByDescending(x => x.RankingPoints)
            .Take(TOP_RANK_LIMIT)
            .ToListAsync();
        return alliances
            .SelectMany(x => x.Members)
            .Where(x => x.UpdatedAt < today)
            .Take(BATCH_SIZE)
            .ToList();
    }
}
