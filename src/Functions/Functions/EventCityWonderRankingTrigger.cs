using Ingweland.Fog.Functions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class EventCityWonderRankingTrigger(
    IEventCityWonderRankingFetcher fetcher,
    ILogger<TopHeroInsightsProcessorTrigger> logger)
{
    [Function(nameof(EventCityWonderRankingTrigger))]
    public Task<bool> Run([ActivityTrigger] object? _)
    {
        logger.LogInformation("{activity} started.", nameof(EventCityWonderRankingTrigger));
        return fetcher.RunAsync();
    }
}
