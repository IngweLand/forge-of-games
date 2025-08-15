using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Providers;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Functions.Services.Interfaces;
using Ingweland.Fog.Functions.Services.Orchestration.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services.Orchestration;

public class TopPlayersUpdateManager(
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
    inGamePlayerService, databaseWarmUpService, logger), ITopPlayersUpdateManager
{
    private const int BATCH_SIZE = 100;
    private const int TOP_RANK_LIMIT = 500;

    protected override Task<List<Player>> GetPlayers(string gameWorldId)
    {
        Logger.LogDebug("Fetching players");

      return GetInitQuery(gameWorldId)
            .Take(BATCH_SIZE)
            .ToListAsync();
    }
    
    protected override async Task<bool> HasMorePlayers(string gameWorldId)
    {
        var count = await GetInitQuery(gameWorldId).CountAsync();
        return count > 0;
    }
    
    private IQueryable<Player> GetInitQuery(string gameWorldId)
    {
        var today = DateTime.UtcNow.ToDateOnly();

        return context.Players
            .Where(x => x.Status == InGameEntityStatus.Active && x.WorldId == gameWorldId)
            .OrderByDescending(x => x.RankingPoints)
            .Take(TOP_RANK_LIMIT)
            .Where(x => x.ProfileUpdatedAt < today);
    }
}
