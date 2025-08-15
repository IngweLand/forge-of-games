using Ingweland.Fog.Functions.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class TopPlayersCityFetcherTrigger(
    ITopPlayersCityFetcher playerCityFetcher,
    ILogger<TopPlayersCityFetcherTrigger> logger)
{
    [Function(nameof(TopPlayersCityFetcherTrigger))]
    public Task<bool> Run([ActivityTrigger] object? _)
    {
        logger.LogInformation("{activity} started.", nameof(TopPlayersCityFetcherTrigger));
        return playerCityFetcher.RunAsync();
    }
}
