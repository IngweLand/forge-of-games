using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services;

public interface IDatabaseCleanupService
{
    Task RunAsync();
}

public class DatabaseCleanupService(IFogDbContext context, ILogger<DatabaseCleanupService> logger)
    : IDatabaseCleanupService
{
    public async Task RunAsync()
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-FogConstants.DisplayedStatsDays).ToDateOnly();
        await CleanupPlayerRankingsAsync(cutoffDate);
        await CleanupAllianceRankingsAsync(cutoffDate);
        await CleanupPvpRankingsAsync(cutoffDate);
        await CleanupPvpBattlesAsync(DateTime.Today.AddDays(-30));
    }

    private async Task CleanupPlayerRankingsAsync(DateOnly cutoffDate)
    {
        var old = await context.PlayerRankings.Where(x => x.CollectedAt < cutoffDate).ToListAsync();
        context.PlayerRankings.RemoveRange(old);
        await context.SaveChangesAsync();
        logger.LogInformation("Removed {OldCount} old player rankings", old.Count);
    }

    private async Task CleanupAllianceRankingsAsync(DateOnly cutoffDate)
    {
        var old = await context.AllianceRankings.Where(x => x.CollectedAt < cutoffDate).ToListAsync();
        context.AllianceRankings.RemoveRange(old);
        await context.SaveChangesAsync();
        logger.LogInformation("Removed {OldCount} old alliance rankings", old.Count);
    }

    private async Task CleanupPvpRankingsAsync(DateOnly cutoffDate)
    {
        var old = await context.PvpRankings.Where(x => x.CollectedAt < cutoffDate).ToListAsync();
        context.PvpRankings.RemoveRange(old);
        await context.SaveChangesAsync();
        logger.LogInformation("Removed {OldCount} old pvp rankings", old.Count);
    }

    private async Task CleanupPvpBattlesAsync(DateTime cutoffDate)
    {
        var total = 0;
        while (true)
        {
            var old = await context.PvpBattles.Where(x => x.PerformedAt < cutoffDate).Take(50).ToListAsync();
            total += old.Count;
            if (old.Count == 0)
            {
                break;
            }
            context.PvpBattles.RemoveRange(old);
            await context.SaveChangesAsync();
            logger.LogInformation("Removed {OldCount} old pvp battles", old.Count);
        }
        logger.LogInformation("Total removed old pvp battles - {TotalCount}", total);
    }
}
