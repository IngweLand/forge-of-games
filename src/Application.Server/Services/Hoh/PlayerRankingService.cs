using AutoMapper;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class PlayerRankingService(IFogDbContext context, IMapper mapper, ILogger<PlayerRankingService> logger)
    : IPlayerRankingService
{
    public async Task AddOrUpdateRangeAsync(IEnumerable<PlayerRank> rankings, string worldId, DateOnly collectedAt,
        PlayerRankingType playerRankingType)
    {
        var rankingList = rankings as IList<PlayerRank> ?? rankings.ToList();
        if (rankingList.Count == 0)
        {
            logger.LogInformation("AddRange called with no player rankings to add.");
            return;
        }

        rankingList = Prepare(rankingList);

        logger.LogInformation("Starting to add/update {PlayerCount} player rankings.", rankingList.Count);

        var existingPlayers = await GetPlayers(rankingList, collectedAt);

        int updatedPlayerCount = 0, addedPlayerCount = 0, updatedRankingCount = 0, addedRankingCount = 0;

        foreach (var source in rankingList)
        {
            var (player, playerRanking) = MapEntities(source, worldId, collectedAt, playerRankingType);

            UpdatePlayerRankingPointsIfRequired(player, playerRankingType);

            if (existingPlayers.TryGetValue(new PlayerKey(playerRanking.WorldId, playerRanking.InGamePlayerId),
                    out var existingPlayer))
            {
                mapper.Map(player, existingPlayer);
                updatedPlayerCount++;
                if (existingPlayer.Rankings.Count == 0)
                {
                    existingPlayer.Rankings.Add(playerRanking);
                    addedRankingCount++;
                }
                else
                {
                    var existingRanking = existingPlayer.Rankings.First();
                    if (playerRanking.Key == existingRanking.Key)
                    {
                        mapper.Map(playerRanking, existingPlayer.Rankings.First());
                        updatedRankingCount++;
                    }
                    else
                    {
                        existingPlayer.Rankings.Add(playerRanking);
                        addedRankingCount++;
                    }
                }
            }
            else
            {
                player.Rankings.Add(playerRanking);
                context.Players.Add(player);
                addedPlayerCount++;
                addedRankingCount++;
            }
        }

        logger.LogInformation(
            "Summary of modifications {@Summary}",
            new {addedPlayerCount, updatedPlayerCount, addedRankingCount, updatedRankingCount});

        await context.SaveChangesAsync();

        logger.LogInformation("Successfully saved changes to database.");
    }

    private async Task<Dictionary<PlayerKey, Player>> GetPlayers(IList<PlayerRank> rankingList, DateOnly date)
    {
        var inGamePlayerIds = rankingList.Select(p => p.Id).ToHashSet();
        var players = await context.Players
            .Include(p => p.Rankings.Where(pr => pr.CollectedAt == date))
            .Where(p => inGamePlayerIds.Contains(p.InGamePlayerId))
            .ToListAsync();
        return players.ToDictionary(p => p.Key);
    }

    private (Player Player, PlayerRanking PlayerRanking) MapEntities(PlayerRank source, string worldId,
        DateOnly collectedAt, PlayerRankingType playerRankingType)
    {
        var playerRanking = mapper.Map<PlayerRanking>(source, opt =>
        {
            opt.Items[ResolutionContextKeys.PLAYER_RANKING_TYPE] = playerRankingType;
            opt.Items[ResolutionContextKeys.WORLD_ID] = worldId;
            opt.Items[ResolutionContextKeys.DATE] = collectedAt;
        });
        var player = mapper.Map<Player>(source, opt =>
        {
            opt.Items[ResolutionContextKeys.WORLD_ID] = worldId;
            opt.Items[ResolutionContextKeys.DATE] = collectedAt;
        });
        return (player, playerRanking);
    }

    private static List<PlayerRank> Prepare(IEnumerable<PlayerRank> rankings)
    {
        return rankings
            .GroupBy(p => p.Id)
            .Select(g => g.MaxBy(p => p.Points)!)
            .ToList();
    }

    private void UpdatePlayerRankingPointsIfRequired(Player player, PlayerRankingType playerRankingType)
    {
        switch (playerRankingType)
        {
            case PlayerRankingType.RankingPoints:
            {
                break;
            }
            case PlayerRankingType.ResearchPoints:
            {
                player.RankingPoints =
                    (int) (player.RankingPoints * HohConstants.RESEARCH_TO_PLAYER_RANKING_POINTS_FACTOR);
                break;
            }
            default:
            {
                logger.LogError("Unsupported player ranking type {playerRankingType}", playerRankingType);
                throw new ArgumentException($"Unsupported player ranking type {playerRankingType}");
            }
        }
    }
}
