using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.InnSdk.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services;

public interface IInGameEventsFetcher
{
    Task RunAsync();
}

public class InGameEventsFetcher(
    IFogDbContext context,
    IInnSdkClient innSdkClient,
    IGameWorldsProvider gameWorldsProvider,
    ILogger<InGameEventsFetcher> logger) : IInGameEventsFetcher
{
    public async Task RunAsync()
    {
        foreach (var gw in gameWorldsProvider.GetGameWorlds())
        {
            logger.LogInformation("Fetching events for world {WorldId}", gw.Id);
            
            var startupData = await innSdkClient.StaticDataService.GetStartupDataAsync(gw);
            if (startupData.IsFailed)
            {
                startupData.Log<InGameEventsFetcher>();
                continue;
            }

            foreach (var ige in startupData.Value.InGameEvents)
            {
                var defId = EventDefinitionId.Undefined;
                try
                {
                    defId = HohStringParser.ParseEnumFromString<EventDefinitionId>(ige.EventDefinition.Id);
                }
                catch
                {
                    // ignore;
                }

                if (defId == EventDefinitionId.Undefined)
                {
                    continue;
                }

                if (await context.InGameEvents.AnyAsync(x =>
                        x.WorldId == gw.Id && x.DefinitionId == defId && x.EventId == ige.Id))
                {
                    continue;
                }

                context.InGameEvents.Add(new InGameEventEntity
                {
                    WorldId = gw.Id,
                    DefinitionId = defId,
                    EventId = ige.Id,
                    StartAt = ige.Start.ToDateTime(),
                    EndAt = ige.End.ToDateTime(),
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
