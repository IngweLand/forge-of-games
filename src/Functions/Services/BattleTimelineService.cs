using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Functions.Services;

public interface IBattleTimelineService
{
    Task UpsertAsync(HeroFinishWaveRequestDto dto);
}

public class BattleTimelineService(IFogDbContext context) : IBattleTimelineService
{
    public async Task UpsertAsync(HeroFinishWaveRequestDto dto)
    {
        var entries = dto.Timeline.Entries
            .Where(x => x.AbilityCaster != null)
            .Select(x => new BattleTimelineEntry
            {
                TimeMillis = x.TimeMilliseconds,
                AbilityId = x.AbilityCaster!.BattleAbilityDefinitionId,
                UnitInBattleId = x.AbilityCaster.Caster.InBattleId,
            })
            .OrderBy(x => x.TimeMillis);

        var battleId = dto.BattleId.ToByteArray();
        var existingTimeline =
            await context.BattleTimelines.FirstOrDefaultAsync(x => x.InGameBattleId == battleId);
        if (existingTimeline != null)
        {
            existingTimeline.Entries.UnionWith(entries);
        }
        else
        {
            var newTimeline = new BattleTimelineEntity
            {
                InGameBattleId = battleId,
                Entries = entries.ToHashSet(),
            };
            context.BattleTimelines.Add(newTimeline);
        }

        await context.SaveChangesAsync();
    }
}
