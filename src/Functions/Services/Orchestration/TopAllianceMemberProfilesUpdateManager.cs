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

public class TopAllianceMemberProfilesUpdateManager(
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
    inGamePlayerService, databaseWarmUpService, logger), ITopAllianceMemberProfilesUpdateManager
{
    private const int BATCH_SIZE = 100;
    private const int TOP_RANK_LIMIT = 40;

    protected override async Task<List<Player>> GetPlayers(string gameWorldId)
    {
        Logger.LogDebug("Fetching players");

        var players = await Query(gameWorldId);
        return players.Take(BATCH_SIZE).ToList();
    }

    protected override async Task<bool> HasMorePlayers(string gameWorldId)
    {
        var players = await Query(gameWorldId);
        return players.Any();
    }

    private async Task<IEnumerable<Player>> Query(string gameWorldId)
    {
        var today = DateTime.UtcNow.ToDateOnly();
        var alliances = await context.Alliances
            .Include(x => x.Members).ThenInclude(x => x.Player)
            .Where(x => x.WorldId == gameWorldId)
            .OrderByDescending(x => x.RankingPoints)
            .Take(TOP_RANK_LIMIT)
            .ToListAsync();
        return alliances
            .SelectMany(x => x.Members.Select(y => y.Player))
            .Where(x => x.ProfileUpdatedAt < today);
    }
}
