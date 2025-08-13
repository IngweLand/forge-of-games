using Ingweland.Fog.Functions.Services.Orchestration;
using Microsoft.Azure.Functions.Worker;

namespace Ingweland.Fog.Functions.Functions;

public class TopHeroInsightsProcessorTrigger(ITopHeroInsightsProcessor processor)
{
    [Function("TopHeroInsightsProcessorTrigger")]
    public async Task Run([TimerTrigger("0 0 6 * * *", RunOnStartup = false)] TimerInfo myTimer)
    {
        await processor.RunAsync();
    }
}
