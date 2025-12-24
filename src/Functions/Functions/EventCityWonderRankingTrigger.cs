using Ingweland.Fog.Functions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class EventCityWonderRankingTrigger(
    IEventCityWonderRankingFetcher fetcher,
    DatabaseWarmUpService databaseWarmUpService,
    ILogger<TopHeroInsightsProcessorTrigger> logger)
{
    [Function(nameof(EventCityWonderRankingTrigger))]
    public async Task<bool> Run([ActivityTrigger] object? _)
    {
        logger.LogInformation("{activity} started.", nameof(EventCityWonderRankingTrigger));
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();
        return await fetcher.RunAsync();
    }
}
