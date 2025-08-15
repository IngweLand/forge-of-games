using Ingweland.Fog.Functions.Services.Orchestration;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class TopHeroInsightsProcessorTrigger(ITopHeroInsightsProcessor processor, ILogger<TopHeroInsightsProcessorTrigger> logger)
{
    [Function(nameof(TopHeroInsightsProcessorTrigger))]
    public async Task<bool> Run([ActivityTrigger] object? _)
    {logger.LogInformation("{activity} started.", nameof(TopHeroInsightsProcessorTrigger));
        await processor.RunAsync();
        return false;
    }
}
