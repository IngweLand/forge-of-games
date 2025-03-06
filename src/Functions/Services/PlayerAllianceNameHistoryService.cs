using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services;

public interface IPlayerAllianceNameHistoryService : IPlayerHistoryServiceBase
{
}

public class PlayerAllianceNameHistoryService(IFogDbContext context, ILogger<PlayerRankingService> logger)
    : PlayerHistoryServiceBase(context, logger), IPlayerAllianceNameHistoryService
{
    protected override Func<IQueryable<Player>, IQueryable<Player>> IncludeQuery =>
        query => query.Include(p => p.AllianceNameHistory).Include(p => p.CurrentAlliance).AsSplitQuery();

    protected override List<PlayerAggregate> Filter(IEnumerable<PlayerAggregate> playerAggregates)
    {
        return playerAggregates.Where(p => !string.IsNullOrEmpty(p.AllianceName)).ToList();
    }

    protected override void ProcessExistingPlayer(Player existingPlayer, IEnumerable<PlayerAggregate> playerAggregates)
    {
        var list = playerAggregates.ToList();
        var existingNames = existingPlayer.AllianceNameHistory.Select(entry => entry.AllianceName).ToHashSet();
        var newNames = list.Select(p => p.AllianceName).ToHashSet().Except(existingNames);
        foreach (var newName in newNames)
        {
            existingPlayer.AllianceNameHistory.Add(new PlayerAllianceNameHistoryEntry() {AllianceName = newName!});
        }

        var latestName = list.OrderByDescending(p => p.CollectedAt).First().AllianceName;
        existingPlayer.AllianceName = latestName;
        if (existingPlayer.CurrentAlliance?.Name != latestName)
        {
            existingPlayer.CurrentAlliance = null;
        }
    }
}