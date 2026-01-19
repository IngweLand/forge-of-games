using Ingweland.Fog.Functions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class EventCityStrategyFactoryTrigger(
    IEventCityStrategyFactory factory,
    DatabaseWarmUpService databaseWarmUpService,
    ILogger<EventCityStrategyFactoryTrigger> logger)
{
    [Function("EventCityStrategyFactoryTrigger")]
    public async Task Run([TimerTrigger("0 0 0 1 1 1", RunOnStartup = false)] TimerInfo myTimer)
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();
        logger.LogDebug("Database warm-up completed");

        await factory.RunAsync();
    }
}
