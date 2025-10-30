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

public interface IAllianceAthService
{
    Task RunAsync(IReadOnlyCollection<HeroTreasureHuntAlliancePoints> rankings, string worldId,
        DateTime collectedAt);
}

public class AllianceAthService(
    IFogDbContext context,
    IMapper mapper,
    IAllianceService allianceService,
    ILogger<AllianceAthService> logger) : IAllianceAthService
{
    public async Task RunAsync(IReadOnlyCollection<HeroTreasureHuntAlliancePoints> rankings, string worldId,
        DateTime collectedAt)
    {
        if (rankings.Count == 0)
        {
            return;
        }
        
        var alliances = mapper.Map<List<AllianceAggregate>>(rankings.Select(x => x.Alliance), opt =>
        {
            opt.Items[ResolutionContextKeys.WORLD_ID] = worldId;
            opt.Items[ResolutionContextKeys.DATE] = collectedAt;
        });
        await allianceService.AddAsync(alliances);

        foreach (var r in rankings)
        {
            var inGameEvent = await context.InGameEvents
                .FirstOrDefaultAsync(x =>
                    x.WorldId == worldId && x.DefinitionId == EventDefinitionId.TreasureHuntLeague &&
                    x.EventId == r.EventId);
            if (inGameEvent == null)
            {
                logger.LogError("In-game event with key {WorldId}:{EventId} not found.", worldId, r.EventId);
                continue;
            }

            var alliance = await context.Alliances
                .Include(x => x.AthRankings.Where(y => y.InGameEventId == inGameEvent.Id))
                .FirstOrDefaultAsync(x => x.WorldId == worldId && x.InGameAllianceId == r.Alliance.Id);
            if (alliance == null)
            {
                logger.LogError("Alliance with key {WorldId}:{InGameAllianceId} not found.", worldId, r.Alliance.Id);
                continue;
            }

            if (alliance.AthRankings.Count > 1)
            {
                // this should never happen because it's enforced on db level, but let's double-check
                logger.LogError("Alliance with key {WorldId}:{InGameAllianceId} has more than one ath ranking.",
                    worldId, r.Alliance.Id);
                continue;
            }
            var athRanking = alliance.AthRankings.FirstOrDefault();
            if (athRanking == null)
            {
                athRanking = new AllianceAthRanking()
                {
                    InGameEventId = inGameEvent.Id,
                    Points = r.Points,
                    League = r.League,
                    CollectedAt = collectedAt,
                };
                alliance.AthRankings.Add(athRanking);
            }
            else if(athRanking.CollectedAt < collectedAt)
            {
                athRanking.Points = r.Points;
                athRanking.CollectedAt = collectedAt;
            }
        }
        
        await context.SaveChangesAsync();
    }
}
