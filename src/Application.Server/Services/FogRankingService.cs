using FluentResults;
using Ingweland.Fog.Application.Server.Errors;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services;

public class FogRankingService(IFogDbContext context, ILogger<FogRankingService> logger) : IFogRankingService
{
    public async Task<Result> UpsertRankingsAsync(string worldId, DateOnly collectedAt,
        IEnumerable<PlayerRank> rankings,
        PlayerRankingType rankingType)
    {
        try
        {
            var latestUnique = rankings
                .GroupBy(x => x.Id)
                .Select(g => g.OrderByDescending(x => x.Points).First())
                .ToList();

            logger.LogDebug("Adding/updating rankings for {PlayerCount} players. Ranking type: {rt}",
                latestUnique.Count,
                rankingType);
            var inGamePlayerIds = latestUnique.Select(p => p.Id).ToHashSet();

            var existingPlayers = await FindExistingPlayers(worldId, collectedAt, inGamePlayerIds, rankingType);
            logger.LogDebug("Found {ExistingPlayerCount} existing players.", existingPlayers.Count);

            foreach (var playerRank in latestUnique)
            {
                if (!existingPlayers.TryGetValue(playerRank.Id, out var player))
                {
                    continue;
                }

                var existingRanking = player.Rankings.FirstOrDefault();
                if (existingRanking != null)
                {
                    if (existingRanking.Points > playerRank.Points)
                    {
                        continue;
                    }

                    existingRanking.Points = playerRank.Points;
                    existingRanking.Rank = playerRank.Rank;
                }
                else
                {
                    player.Rankings.Add(new PlayerRanking
                    {
                        Points = playerRank.Points,
                        Rank = playerRank.Rank,
                        CollectedAt = collectedAt,
                        Type = rankingType,
                    });
                }
            }

            await context.SaveChangesAsync();

            logger.LogDebug("Successfully updated rankings for {PlayerCount} players. Ranking type: {rt}",
                existingPlayers.Count, rankingType);

            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail(new RankingsUpsertionError(e));
        }
    }

    private async Task<Dictionary<int, Player>> FindExistingPlayers(string worldId, DateOnly collectedAt,
        HashSet<int> inGamePlayerIds, PlayerRankingType rankingType)
    {
        var players = await context.Players
            .Include(p => p.Rankings.Where(pr => pr.CollectedAt == collectedAt && pr.Type == rankingType))
            .Where(p => inGamePlayerIds.Contains(p.InGamePlayerId))
            .ToListAsync();
        var result = players.Where(x => x.WorldId == worldId).ToDictionary(x => x.InGamePlayerId);
        return result;
    }
}
