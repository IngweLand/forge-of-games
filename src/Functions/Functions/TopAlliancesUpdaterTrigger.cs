using Ingweland.Fog.Functions.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class TopAlliancesUpdaterTrigger(
    ITopAlliancesUpdateManager topAlliancesUpdateManager,
    ILogger<TopHeroInsightsProcessorTrigger> logger)
{
    [Function(nameof(TopAlliancesUpdaterTrigger))]
    public Task<bool> Run([ActivityTrigger] object? _)
    {
        logger.LogInformation("{activity} started.", nameof(TopAlliancesUpdaterTrigger));
        return topAlliancesUpdateManager.RunAsync();
    }
}
