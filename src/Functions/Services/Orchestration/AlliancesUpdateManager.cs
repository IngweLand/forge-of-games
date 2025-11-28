using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Functions.Services.Interfaces;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services.Orchestration;

public class AlliancesUpdateManager(
    IGameWorldsProvider gameWorldsProvider,
    IFogDbContext context,
    IAllianceUpdateOrchestrator allianceUpdateOrchestrator,
    DatabaseWarmUpService databaseWarmUpService,
    ILogger<AlliancesUpdateManager> logger) : IAlliancesUpdateManager
{
    private const int BATCH_SIZE = 100;
    protected IFogDbContext Context { get; } = context;
    protected ILogger<AlliancesUpdateManager> Logger { get; } = logger;

    public async Task<bool> RunAsync()
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();
        Logger.LogDebug("Database warm-up completed");

        var hasMoreAlliances = false;
        foreach (var gameWorld in gameWorldsProvider.GetGameWorlds())
        {
            var allianceIds = await GetAlliances(gameWorld.Id);
            Logger.LogInformation("Retrieved {AllianceCount} alliances to process from the world {world}",
                allianceIds.Count, gameWorld.Id);
            foreach (var id in allianceIds)
            {
                Logger.LogDebug("Processing alliance {@id}", id);
                var delayTask = Task.Delay(500);
                var result = await allianceUpdateOrchestrator.UpdateAsync(id, CancellationToken.None);

                result.LogIfFailed<AlliancesUpdateManager>();

                await delayTask;
            }

            hasMoreAlliances = hasMoreAlliances || await HasMoreAlliances(gameWorld.Id);
        }

        return hasMoreAlliances;
    }

    private Task<bool> HasMoreAlliances(string worldId)
    {
        return GetInitQuery(worldId).AnyAsync();
    }

    protected virtual IQueryable<int> GetInitQuery(string worldId)
    {
        Logger.LogDebug("Getting alliances from the database for world {worldId}.", worldId);
        var today= DateTime.UtcNow.ToDateOnly();

        return Context.Alliances.AsNoTracking()
            .Where(x => x.WorldId == worldId && x.Status == InGameEntityStatus.Active && x.UpdatedAt < today)
            .OrderBy(x => x.UpdatedAt)
            .Select(x => x.Id);
    }

    private Task<List<int>> GetAlliances(string worldId)
    {
        Logger.LogDebug("Getting alliances from the database for world {worldId}.", worldId);

        return GetInitQuery(worldId)
            .Take(BATCH_SIZE)
            .ToListAsync();
    }
}
