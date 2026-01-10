using Ingweland.Fog.Functions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class EventCityFetcherTrigger(
    IEventCityFetcher cityFetcher,
    DatabaseWarmUpService databaseWarmUpService,
    ILogger<EventCityFetcherTrigger> logger)
{
    [Function("EventCityFetcherTrigger")]
    public async Task Run([TimerTrigger("0 */15 * * * *")] TimerInfo myTimer)
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();
        logger.LogDebug("Database warm-up completed");

        await cityFetcher.RunAsync();
    }
}
