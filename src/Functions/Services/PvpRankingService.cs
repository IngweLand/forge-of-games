using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Player = Ingweland.Fog.Models.Fog.Entities.Player;

namespace Ingweland.Fog.Functions.Services;

public interface IPvpRankingService
{
    Task AddOrUpdateRankingsAsync(IEnumerable<PlayerAggregate> playerAggregates);
}

public class PvpRankingService(IFogDbContext context, IMapper mapper, ILogger<PvpRankingService> logger)
    : IPvpRankingService
{
    private const int TimeBucketHours = 12;

    public async Task AddOrUpdateRankingsAsync(IEnumerable<PlayerAggregate> playerAggregates)
    {
        var filtered = playerAggregates.Where(p => p.CanBeConvertedToPvpRanking()).ToList();
        if (filtered.Count == 0)
        {
            return;
        }

        var latestUnique = filtered
            .GroupBy(p => (p.WorldId, p.InGamePlayerId, p.PlayerRankingType, AdjustCollectedTime(p.CollectedAt)))
            .Select(g => g.OrderByDescending(p => p.CollectedAt).First())
            .ToList();

        int updatedPlayerCount = 0, updatedRankingCount = 0, addedRankingCount = 0;

        foreach (var chunk in latestUnique.Chunk(1000))
        {
            var inGamePlayerIds = chunk.Select(p => p.InGamePlayerId).ToHashSet();
            var earliestDate = chunk.OrderBy(p => p.CollectedAt).Select(p => p.CollectedAt).First();

            var existingPlayers = await FindExistingPlayers(inGamePlayerIds, earliestDate);

            foreach (var playerAggregate in chunk)
            {
                if (existingPlayers.TryGetValue(playerAggregate.Key, out var existingPlayer))
                {
                    var date = AdjustCollectedTime(playerAggregate.CollectedAt);
                    var existingRanking = existingPlayer.PvpRankings.FirstOrDefault(r => r.CollectedAt == date);
                    if (existingRanking != null)
                    {
                        existingRanking.Points = playerAggregate.PvpRankingPoints!.Value;
                        existingRanking.Rank = playerAggregate.PvpRank!.Value;
                        updatedRankingCount++;
                    }
                    else
                    {
                        existingPlayer.PvpRankings.Add(new PvpRanking()
                        {
                            Rank = playerAggregate.PvpRank!.Value,
                            Points = playerAggregate.PvpRankingPoints!.Value,
                            CollectedAt = date,
                        });
                        addedRankingCount++;
                    }
                }
                else
                {
                    // log warning
                }

                await context.SaveChangesAsync();
            }
        }
    }

    private static DateTime AdjustCollectedTime(DateTime src)
    {
        var hour = src.Hour / TimeBucketHours * TimeBucketHours;
        return new DateTime(src.Year, src.Month, src.Day, hour, 0, 0);
    }

    private async Task<Dictionary<PlayerKey, Player>> FindExistingPlayers(HashSet<int> inGamePlayerIds,
        DateTime earliestDate)
    {
        var date = earliestDate.AddDays(-1);
        var players = await context.Players
            .Include(p => p.PvpRankings.Where(pr => pr.CollectedAt > date))
            .Where(p => inGamePlayerIds.Contains(p.InGamePlayerId))
            .ToListAsync();
        return players.ToDictionary(p => p.Key);
    }
}