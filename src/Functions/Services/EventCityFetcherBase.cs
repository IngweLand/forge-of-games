using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Functions.Services;

public abstract class EventCityFetcherBase(IFogDbContext context)
{
    protected IFogDbContext Context { get; } = context;

    protected async Task<InGameEventEntity?> GetCurrentEvent(string worldId)
    {
        var now = DateTime.UtcNow;
        var e = await Context.InGameEvents.FirstOrDefaultAsync(x =>
            x.DefinitionId == EventDefinitionId.EventCity && x.WorldId == worldId && x.StartAt <= now &&
            x.EndAt >= now);
        if (e == null || e.EndAt.Date != now.Date)
        {
            return null;
        }

        return e;
    }
}
