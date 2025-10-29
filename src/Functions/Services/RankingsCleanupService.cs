using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Functions.Services;

public interface IRankingsCleanupService
{
    Task RunAsync();
}

public class RankingsCleanupService(IFogDbContext context) : IRankingsCleanupService
{
    public async Task RunAsync()
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-FogConstants.DisplayedStatsDays).ToDateOnly();
        await CleanupPlayerRankingsAsync(cutoffDate);
        await CleanupAllianceRankingsAsync(cutoffDate);
    }

    private async Task CleanupPlayerRankingsAsync(DateOnly cutoffDate)
    {
        var oldRankings = await context.PlayerRankings.Where(x => x.CollectedAt < cutoffDate).ToListAsync();
        context.PlayerRankings.RemoveRange(oldRankings);
        await context.SaveChangesAsync();
    }

    private async Task CleanupAllianceRankingsAsync(DateOnly cutoffDate)
    {
        var oldRankings = await context.AllianceRankings.Where(x => x.CollectedAt < cutoffDate).ToListAsync();
        context.AllianceRankings.RemoveRange(oldRankings);
        await context.SaveChangesAsync();
    }
}
