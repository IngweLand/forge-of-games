using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Functions.Services;

public interface IBattleTimelineService
{
    Task UpsertAsync(IEnumerable<HeroFinishWaveRequestDto> dtos);
}

public class BattleTimelineService(IFogDbContext context) : IBattleTimelineService
{
    public async Task UpsertAsync(IEnumerable<HeroFinishWaveRequestDto> dtos)
    {
        var timelines = dtos.GroupBy(x => x.BattleId)
            .Select(g =>
            {
                return (BattleId: g.Key, Entries: g.SelectMany(x => x.Timeline.Entries)
                    .Where(x => x.AbilityCaster != null)
                    .Select(x => new BattleTimelineEntry
                    {
                        TimeMillis = x.TimeMilliseconds,
                        AbilityId = x.AbilityCaster!.BattleAbilityDefinitionId,
                        UnitInBattleId = x.AbilityCaster.Caster.InBattleId,
                    })
                    .OrderBy(x => x.TimeMillis));
            })
            .ToList();

        foreach (var timeline in timelines)
        {
            var battleId = timeline.BattleId.ToByteArray();
            var existingTimeline =
                await context.BattleTimelines.FirstOrDefaultAsync(x => x.InGameBattleId == battleId);
            if (existingTimeline != null)
            {
                existingTimeline.Entries.UnionWith(timeline.Entries);
            }
            else
            {
                var newTimeline = new BattleTimelineEntity()
                {
                    InGameBattleId = battleId,
                    Entries = timeline.Entries.ToHashSet(),
                };
                context.BattleTimelines.Add(newTimeline);
            }
            
            await context.SaveChangesAsync();
        }
    }
}
