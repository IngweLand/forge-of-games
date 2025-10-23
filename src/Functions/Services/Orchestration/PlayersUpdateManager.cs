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

    protected override async Task<List<Player>> GetPlayers(string gameWorldId)
    {
        Logger.LogDebug("Fetching players");

        var today = DateTime.UtcNow.ToDateOnly();
        var week = today.AddDays(-7);

        var players = await context.Players
            .Where(x => x.Status == InGameEntityStatus.Active && x.WorldId == gameWorldId)
            .Where(x => (x.Rank == null || x.RankingPoints == null) && x.ProfileUpdatedAt < today)
            .Take(BATCH_SIZE)
            .ToListAsync();

        if (players.Count < BATCH_SIZE)
        {
            players.AddRange(await context.Players
                .Where(x => x.Status == InGameEntityStatus.Active && x.WorldId == gameWorldId && x.ProfileUpdatedAt < week)
                .OrderBy(x => x.ProfileUpdatedAt)
                .Take(BATCH_SIZE)
                .ToListAsync());
        }

        return players.Take(BATCH_SIZE).ToList();
    }

    protected override Task<bool> HasMorePlayers(string gameWorldId)
    {
        // The total amount of players is big enough to assume that we always have more.
        return Task.FromResult(true);
    }
}
