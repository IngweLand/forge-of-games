using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Functions.Services.Interfaces;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services.Orchestration;

public class TopAlliancesUpdateManager(
    IGameWorldsProvider gameWorldsProvider,
    IFogDbContext context,
    IAllianceUpdateOrchestrator allianceUpdateOrchestrator,
    DatabaseWarmUpService databaseWarmUpService,
    ILogger<AlliancesUpdateManager> logger)
    : AlliancesUpdateManager(gameWorldsProvider, context, allianceUpdateOrchestrator, databaseWarmUpService,
        logger), ITopAlliancesUpdateManager
{
    private const int TOP_ALLIANCE_RANK_LIMIT = 500;

    protected override IQueryable<int> GetInitQuery(string worldId)
    {
        var today = DateTime.UtcNow.ToDateOnly();
        return Context.Alliances.AsNoTracking()
            .Where(x => x.WorldId == worldId && x.Status == InGameEntityStatus.Active)
            .OrderByDescending(x => x.RankingPoints)
            .Take(TOP_ALLIANCE_RANK_LIMIT)
            .Where(x => x.UpdatedAt < today)
            .Select(x => x.Id);
    }
}
