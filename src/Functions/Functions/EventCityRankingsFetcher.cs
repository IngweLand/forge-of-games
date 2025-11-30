using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Azure.Functions.Worker;

namespace Ingweland.Fog.Functions.Functions;

public class EventCityRankingsFetcher(
    IRankingUpdateOrchestrator orchestrator,
    IGameWorldsProvider gameWorldsProvider,
    DatabaseWarmUpService databaseWarmUpService)
{
    [Function("EventCityRankingsFetcher")]
    public async Task Run([TimerTrigger("0 0 */3 * * *")] TimerInfo myTimer)
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();

        var hasErrors = false;
        foreach (var gw in gameWorldsProvider.GetGameWorlds())
        {
            var r = await orchestrator.FetchAndStoreRankingAsync(gw, PlayerRankingType.EventCityProgress);
            r.LogIfFailed<EventCityRankingsFetcher>();
            hasErrors |= r.IsFailed;
        }

        if (hasErrors)
        {
            throw new Exception("One or more errors occurred while fetching rankings.");
        }
    }
}
