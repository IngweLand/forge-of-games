using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Functions.Services.Interfaces;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services.Orchestration;

public class TopAllianceMemberUpdateManager(
    IGameWorldsProvider gameWorldsProvider,
    IFogDbContext context,
    IAllianceUpdateOrchestrator allianceUpdateOrchestrator,
    DatabaseWarmUpService databaseWarmUpService,
    ILogger<PlayersUpdateManager> logger) : AllianceMembersUpdateManager(gameWorldsProvider, context,
    allianceUpdateOrchestrator, databaseWarmUpService, logger), ITopAllianceMemberUpdateManager
{
    private const int TOP_ALLIANCE_RANK_LIMIT = 500;
    private const int BATCH_SIZE = 100;

    protected override async Task<bool> HasMoreAlliances(string worldId)
    {
        return await GetInitQuery(worldId).AnyAsync();
    }

    protected override Task<List<int>> GetAlliances(string worldId)
    {
        Logger.LogDebug("Getting alliances from the database for world {worldId}.", worldId);

        return GetInitQuery(worldId)
            .Select(x => x.Id)
            .ToListAsync();
    }

    private IQueryable<Alliance> GetInitQuery(string worldId)
    {
        var today = DateTime.Today.ToUniversalTime();
        return Context.Alliances.AsNoTracking()
            .Where(x => x.WorldId == worldId && x.Status == InGameEntityStatus.Active)
            .OrderByDescending(x => x.RankingPoints)
            .Take(TOP_ALLIANCE_RANK_LIMIT)
            .Where(x => x.MembersUpdatedAt < today)
            .Take(BATCH_SIZE);
    }
}
