using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services;

public interface IAllianceMembersUpdaterService
{
    Task UpdateAsync(IEnumerable<PlayerAggregate> playerAggregates);
}

public class AllianceMembersUpdaterService(IFogDbContext context, ILogger<AllianceMembersUpdaterService> logger)
    : IAllianceMembersUpdaterService
{
    public async Task UpdateAsync(IEnumerable<PlayerAggregate> playerAggregates)
    {
        var unique = playerAggregates
            .Where(p => p.CanBeConvertedToPlayer())
            .OrderByDescending(p => p.CollectedAt)
            .DistinctBy(p => p.Key);

        foreach (var p in unique)
        {
            var player = await context.Players
                .Include(x => x.CurrentAlliance)
                .Include(x => x.AllianceHistory)
                .Include(x => x.LedAlliance)
                .FirstOrDefaultAsync(x => x.WorldId == p.WorldId && x.InGamePlayerId == p.InGamePlayerId);
            if (player == null)
            {
                continue;
            }

            if (p.InGameAllianceId == null)
            {
                player.CurrentAlliance = null;
                player.LedAlliance = null;
                player.AllianceName = null;
                player.UpdatedAt = p.CollectedAt.ToDateOnly();
            }
            else
            {
                var alliance = await context.Alliances
                    .FirstOrDefaultAsync(a =>
                        a.WorldId == p.WorldId && a.InGameAllianceId == p.InGameAllianceId);
                if (alliance != null)
                {
                    player.CurrentAlliance = alliance;
                    player.AllianceName = alliance.Name;
                    if (player.AllianceHistory.All(a => a.Id != alliance.Id))
                    {
                        player.AllianceHistory.Add(alliance);
                    }

                    player.UpdatedAt = p.CollectedAt.ToDateOnly();
                }
            }
        }

        await context.SaveChangesAsync();
    }
}
