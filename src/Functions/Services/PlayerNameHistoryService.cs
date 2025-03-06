using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services;

public interface IPlayerNameHistoryService
{
    Task UpdateAsync(IEnumerable<PlayerAggregate> playerAggregates);
}

public class PlayerNameHistoryService(IFogDbContext context, ILogger<PlayerRankingService> logger)
    : PlayerHistoryServiceBase(context, logger), IPlayerNameHistoryService
{
    protected override Func<IQueryable<Player>, IQueryable<Player>> IncludeQuery =>
        query => query.Include(p => p.NameHistory);

    protected override List<PlayerAggregate> Filter(IEnumerable<PlayerAggregate> playerAggregates)
    {
        return playerAggregates.Where(p => !string.IsNullOrEmpty(p.Name)).ToList();
    }

    protected override void ProcessExistingPlayer(Player existingPlayer, IEnumerable<PlayerAggregate> playerAggregates)
    {
        var list = playerAggregates.ToList();
        var existingNames = existingPlayer.NameHistory.Select(entry => entry.Name).ToHashSet();
        var newNames = list.Select(p => p.Name).ToHashSet().Except(existingNames);
        foreach (var newName in newNames)
        {
            existingPlayer.NameHistory.Add(new PlayerNameHistoryEntry() {Name = newName});
        }
                    
        existingPlayer.Name = list.OrderByDescending(p => p.CollectedAt).First().Name;
    }
}