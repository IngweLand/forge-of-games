using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Functions.Services.Interfaces;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services.Orchestration;

public class TopAllianceMemberUpdateManager(
    IGameWorldsProvider gameWorldsProvider,
    IFogDbContext context,
    IAllianceUpdateOrchestrator allianceUpdateOrchestrator,
    DatabaseWarmUpService databaseWarmUpService,
    ILogger<PlayersUpdateManager> logger) : ITopAllianceMemberUpdateManager
{
    private const int TOP_ALLIANCE_RANK_LIMIT = 100;

    public async Task RunAsync()
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();
        logger.LogDebug("Database warm-up completed");

        foreach (var gameWorld in gameWorldsProvider.GetGameWorlds())
        {
            var allianceIds = await GetAlliances(gameWorld.Id);
            logger.LogInformation("Retrieved {PlayerCount} alliances to process", allianceIds.Count);
            foreach (var id in allianceIds)
            {
                logger.LogDebug("Processing alliance {@id}", id);
                var delayTask = Task.Delay(1000);
                var result = await allianceUpdateOrchestrator.UpdateMembersAsync(id, CancellationToken.None);
                result.LogIfFailed<TopAllianceMemberUpdateManager>();

                await delayTask;
            }
        }
    }

    private Task<List<int>> GetAlliances(string worldId)
    {
        logger.LogDebug("Getting alliances from the database for world {worldId}.", worldId);

        return context.Alliances.AsNoTracking()
            .Where(x => x.WorldId == worldId && x.Status == InGameEntityStatus.Active)
            .OrderByDescending(x => x.RankingPoints)
            .Take(TOP_ALLIANCE_RANK_LIMIT)
            .Select(x => x.Id)
            .ToListAsync();
    }
}
