using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Ranking;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services;

public interface IPlayerAthService
{
    Task RunAsync(IReadOnlyCollection<HeroTreasureHuntPlayerPoints> rankings, string worldId,
        DateTime collectedAt);
}

public class PlayerAthService(
    IFogDbContext context,
    IMapper mapper,
    IPlayerService playerService,
    ILogger<PlayerAthService> logger) : IPlayerAthService
{
    private List<InGameEventEntity> _events = new List<InGameEventEntity>();

    public async Task RunAsync(IReadOnlyCollection<HeroTreasureHuntPlayerPoints> rankings, string worldId,
        DateTime collectedAt)
    {
        if (rankings.Count == 0)
        {
            return;
        }

        var players = mapper.Map<List<PlayerAggregate>>(rankings.Select(x => x.Player), opt =>
        {
            opt.Items[ResolutionContextKeys.WORLD_ID] = worldId;
            opt.Items[ResolutionContextKeys.DATE] = collectedAt;
        });
        await playerService.AddAsync(players);

        foreach (var r in rankings)
        {
            var inGameEvent = _events
                    .FirstOrDefault(x =>
                        x.WorldId == worldId && x.DefinitionId == EventDefinitionId.TreasureHuntLeague &&
                        x.EventId == r.TreasureHuntEventId) ??
                await context.InGameEvents
                    .FirstOrDefaultAsync(x =>
                        x.WorldId == worldId && x.DefinitionId == EventDefinitionId.TreasureHuntLeague &&
                        x.EventId == r.TreasureHuntEventId);
            if (inGameEvent == null)
            {
                logger.LogError("In-game event with key {WorldId}:{EventId} not found.", worldId,
                    r.TreasureHuntEventId);
                continue;
            }

            var player = await context.Players
                .Include(x => x.AthRankings.Where(y => y.InGameEventId == inGameEvent.Id))
                .FirstOrDefaultAsync(x => x.WorldId == worldId && x.InGamePlayerId == r.Player.Id);
            if (player == null)
            {
                logger.LogError("Player with key {WorldId}:{InGamePlayerId} not found.", worldId, r.Player.Id);
                continue;
            }

            if (player.AthRankings.Count > 1)
            {
                // this should never happen because it's enforced on db level, but let's double-check
                logger.LogError("Player with key {WorldId}:{InGamePlayerId} has more than one ath ranking.",
                    worldId, r.Player.Id);
                continue;
            }

            var athRanking = player.AthRankings.FirstOrDefault();
            if (athRanking == null)
            {
                athRanking = new PlayerAthRanking
                {
                    InGameEventId = inGameEvent.Id,
                    Points = r.RankingPoints,
                    UpdatedAt = r.UpdatedAt,
                };
                player.AthRankings.Add(athRanking);
            }
            else if (athRanking.UpdatedAt < r.UpdatedAt)
            {
                athRanking.Points = r.RankingPoints;
                athRanking.UpdatedAt = r.UpdatedAt;
            }
        }

        await context.SaveChangesAsync();
    }
}
