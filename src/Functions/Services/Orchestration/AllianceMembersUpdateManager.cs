using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Functions.Services.Interfaces;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services.Orchestration;

public class AllianceMembersUpdateManager(
    IGameWorldsProvider gameWorldsProvider,
    IFogDbContext context,
    IAllianceUpdateOrchestrator allianceUpdateOrchestrator,
    DatabaseWarmUpService databaseWarmUpService,
    ILogger<PlayersUpdateManager> logger):IAllianceMembersUpdateManager
{
    private const int BATCH_SIZE = 100;
    protected IFogDbContext Context { get; } = context;
    protected ILogger<PlayersUpdateManager> Logger { get; } = logger;

    public async Task<bool> RunAsync()
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();
        Logger.LogDebug("Database warm-up completed");

        var hasMoreAlliances = false;
        foreach (var gameWorld in gameWorldsProvider.GetGameWorlds())
        {
            var allianceIds = await GetAlliances(gameWorld.Id);
            hasMoreAlliances = hasMoreAlliances || await HasMoreAlliances(gameWorld.Id);
            Logger.LogInformation("Retrieved {PlayerCount} alliances to process from the world {world}", allianceIds.Count, gameWorld.Id);
            foreach (var id in allianceIds)
            {
                Logger.LogDebug("Processing alliance {@id}", id);
                var delayTask = Task.Delay(1000);
                var result = await allianceUpdateOrchestrator.UpdateMembersAsync(id, CancellationToken.None);
                result.LogIfFailed<AllianceMembersUpdateManager>();

                await delayTask;
            }
        }

        return hasMoreAlliances;
    }

    protected virtual Task<bool> HasMoreAlliances(string worldId)
    {
        return Task.FromResult(true);
    }

    protected virtual async Task<List<int>> GetAlliances(string worldId)
    {
        Logger.LogDebug("Getting alliances from the database for world {worldId}.", worldId);
        var week = DateTime.UtcNow.AddDays(-7);

        var alliances = await Context.Alliances.AsNoTracking()
            .Where(x => x.WorldId == worldId && x.Status == InGameEntityStatus.Active && x.RankingPoints == 0)
            .Take(BATCH_SIZE)
            .Select(x => x.Id)
            .ToListAsync();

        if (alliances.Count < BATCH_SIZE)
        {
            alliances.AddRange(await Context.Alliances.AsNoTracking()
                .Where(x => x.WorldId == worldId && x.Status == InGameEntityStatus.Active && x.MembersUpdatedAt < week)
                .OrderBy(x => Guid.NewGuid())
                .Take(BATCH_SIZE)
                .Select(x => x.Id)
                .ToListAsync());
        }

        return alliances.Take(BATCH_SIZE).ToList();
    }
}
