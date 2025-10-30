using Ingweland.Fog.Functions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class InGameEventsFetcherTrigger(
    IInGameEventsFetcher gameEventsFetcher,
    ILogger<InGameEventsFetcherTrigger> logger)
{
    [Function(nameof(InGameEventsFetcherTrigger))]
    public async Task<bool> Run([ActivityTrigger] object? _)
    {
        logger.LogInformation("{activity} started.", nameof(InGameEventsFetcherTrigger));
        await gameEventsFetcher.RunAsync();
        return false;
    }
}
