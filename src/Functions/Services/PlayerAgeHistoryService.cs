using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Functions.Extensions;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services;

public interface IPlayerAgeHistoryService
{
    Task UpdateAsync(IEnumerable<PlayerAggregate> playerAggregates);
}

public class PlayerAgeHistoryService(IFogDbContext context, ILogger<PlayerRankingService> logger)
    : PlayerHistoryServiceBase(context, logger), IPlayerAgeHistoryService
{
    protected override Func<IQueryable<Player>, IQueryable<Player>> IncludeQuery =>
        query => query.Include(p => p.AgeHistory);

    protected override List<PlayerAggregate> Filter(IEnumerable<PlayerAggregate> playerAggregates)
    {
        return playerAggregates.Where(p => p.Age != null).ToList();
    }

    protected override void ProcessExistingPlayer(Player existingPlayer, IEnumerable<PlayerAggregate> playerAggregates)
    {
        var combined = existingPlayer.AgeHistory.Select(entry => (entry.Age, entry.ChangedAt))
            .Concat(playerAggregates.Select(p => (p.Age!, p.CollectedAt)));
        var aggregated = combined.Aggregate();

        foreach (var t in aggregated)
        {
            if (existingPlayer.AgeHistory.Any(entry => (entry.Age, entry.ChangedAt) == t))
            {
                continue;
            }

            existingPlayer.AgeHistory.Add(new PlayerAgeHistoryEntry {Age = t.Value, ChangedAt = t.CollectedAt});
            existingPlayer.Age = t.Value;
        }
    }
}