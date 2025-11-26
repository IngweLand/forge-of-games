using FluentResults.Extensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Functions.Services.Interfaces;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.InnSdk.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Errors;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AllianceRankingType = Ingweland.Fog.Models.Hoh.Enums.AllianceRankingType;

namespace Ingweland.Fog.Functions.Services.Orchestration;

public class AlliancesUpdateManager(
    IGameWorldsProvider gameWorldsProvider,
    IFogDbContext context,
    IInnSdkClient innSdkClient,
    IFogAllianceService fogAllianceService,
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
            var alliances = await GetAlliances(gameWorld.Id);
            Logger.LogInformation("Retrieved {AllianceCount} alliances to process from the world {world}",
                alliances.Count, gameWorld.Id);
            foreach (var alliance in alliances)
            {
                Logger.LogDebug("Processing alliance {@id}", alliance.Id);
                var delayTask = Task.Delay(500);
                var result = await innSdkClient.AllianceService.GetAllianceAsync(gameWorld, alliance.InGameAllianceId)
                    .Bind(allianceWithLeader => fogAllianceService.UpsertAlliance(allianceWithLeader.Alliance,
                        gameWorld.Id, DateTime.UtcNow, AllianceRankingType.MemberTotal));

                if (result.HasError<HohSoftError>(x => x.Error == SoftErrorType.AllianceNotFound))
                {
                    logger.LogInformation("Alliance {AllianceId} not found, marking as Missing", alliance.Id);
                    var statusUpdateResult =
                        await fogAllianceService.SetAllianceMissingStatus(alliance.Id, CancellationToken.None);
                    if (statusUpdateResult.IsFailed)
                    {
                        result.WithErrors(statusUpdateResult.Errors);
                    }
                }

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

    protected virtual IQueryable<Alliance> GetInitQuery(string worldId)
    {
        Logger.LogDebug("Getting alliances from the database for world {worldId}.", worldId);
        var week = DateTime.UtcNow.AddDays(-7).ToDateOnly();

        return Context.Alliances.AsNoTracking()
            .Where(x => x.WorldId == worldId && x.Status == InGameEntityStatus.Active && x.UpdatedAt < week)
            .OrderBy(x => x.UpdatedAt);
    }

    private Task<List<Alliance>> GetAlliances(string worldId)
    {
        Logger.LogDebug("Getting alliances from the database for world {worldId}.", worldId);

        return GetInitQuery(worldId)
            .Take(BATCH_SIZE)
            .ToListAsync();
    }
}
