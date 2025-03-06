using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Functions.Extensions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services;

public interface IAllianceNameHistoryService
{
    Task UpdateAsync(IEnumerable<AllianceAggregate> allianceAggregates);
}

public class AllianceNameHistoryService(IFogDbContext context, ILogger<PlayerRankingService> logger)
    : IAllianceNameHistoryService
{
    public async Task UpdateAsync(IEnumerable<AllianceAggregate> allianceAggregates)
    {
        var filtered = Filter(allianceAggregates);

        if (filtered.Count == 0)
        {
            return;
        }

        var grouped = filtered.GroupBy(p => p.Key);

        foreach (var chunk in grouped.Chunk(1000))
        {
            var inGameAllianceIds = chunk.Select(g => g.Key.InGameAllianceId).ToHashSet();
            var existingAlliances = await FindExistingAlliances(inGameAllianceIds);

            foreach (var allianceGroup in chunk)
            {
                if (existingAlliances.TryGetValue(allianceGroup.Key, out var existingAlliance))
                {
                    ProcessExistingAlliance(existingAlliance, allianceGroup);
                }
                else
                {
                    // log warning 
                }
            }

            await context.SaveChangesAsync();
        }
    }

    private List<AllianceAggregate> Filter(IEnumerable<AllianceAggregate> allianceAggregates)
    {
        return allianceAggregates.Where(a => !string.IsNullOrWhiteSpace(a.Name)).ToList();
    }

    private void ProcessExistingAlliance(Alliance existingAlliance, IEnumerable<AllianceAggregate> allianceAggregates)
    {
        var combined = existingAlliance.NameHistory.Select(entry => (entry.Name, entry.ChangedAt))
            .Concat(allianceAggregates.Select(p => (p.Name, p.CollectedAt)));
        var aggregated = combined.Aggregate();

        foreach (var t in aggregated)
        {
            if (existingAlliance.NameHistory.Any(entry => (entry.Name, entry.ChangedAt) == t))
            {
                continue;
            }

            existingAlliance.NameHistory.Add(new AllianceNameHistoryEntry()
                {Name = t.Value, ChangedAt = t.CollectedAt});
            existingAlliance.Name = t.Value;
        }
    }

    private async Task<Dictionary<AllianceKey, Alliance>> FindExistingAlliances(HashSet<int> inGamePlayerIds)
    {
        var players = await context.Alliances
            .Include(a => a.NameHistory)
            .Where(p => inGamePlayerIds.Contains(p.InGameAllianceId))
            .ToListAsync();
        return players.ToDictionary(p => p.Key);
    }
}