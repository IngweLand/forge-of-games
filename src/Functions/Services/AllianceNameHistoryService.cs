using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Functions.Extensions;
using Ingweland.Fog.Models.Fog.Entities;
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
        logger.LogInformation("Filtered alliance aggregates count: {FilteredCount}", filtered.Count);

        if (filtered.Count == 0)
        {
            logger.LogInformation("No valid alliance aggregates to process after filtering.");
            return;
        }

        var grouped = filtered.GroupBy(p => p.Key);

        foreach (var chunk in grouped.Chunk(1000))
        {
            logger.LogInformation("Processing a chunk containing {ChunkCount} alliance groups", chunk.Count());
            var inGameAllianceIds = chunk.Select(g => g.Key.InGameAllianceId).ToHashSet();
            var existingAlliances = await FindExistingAlliances(inGameAllianceIds);

            logger.LogInformation("Retrieved {ExistingCount} existing alliances for the current chunk",
                existingAlliances.Count);

            foreach (var allianceGroup in chunk)
            {
                if (existingAlliances.TryGetValue(allianceGroup.Key, out var existingAlliance))
                {
                    ProcessExistingAlliance(existingAlliance, allianceGroup);
                }
                else
                {
                    logger.LogWarning("Alliance not found for key {AllianceKey}. Skipping processing.",
                        allianceGroup.Key);
                }
            }

            logger.LogInformation("Saving changes for the current chunk.");
            await context.SaveChangesAsync();
            logger.LogInformation("Successfully saved changes for the current chunk.");
        }

        logger.LogInformation("UpdateAsync completed processing alliance aggregates.");
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

    private async Task<Dictionary<AllianceKey, Alliance>> FindExistingAlliances(HashSet<int> inGameAllianceIds)
    {
        var alliances = await context.Alliances
            .Include(a => a.NameHistory)
            .Where(p => inGameAllianceIds.Contains(p.InGameAllianceId))
            .ToListAsync();
        logger.LogDebug("Retrieved {AlliancesCount} alliances from the database for in-game alliance IDs.",
            alliances.Count);
        return alliances.ToDictionary(p => p.Key);
    }
}