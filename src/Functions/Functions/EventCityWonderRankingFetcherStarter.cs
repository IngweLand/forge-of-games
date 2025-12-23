using Ingweland.Fog.Functions.Functions.Orchestration;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class EventCityWonderRankingFetcherStarter(ILogger<EventCityWonderRankingFetcherStarter> logger)
{
    [Function("EventCityWonderRankingFetcherStarter")]
    public async Task Run([TimerTrigger("0 30 11 * * *", RunOnStartup = false)] TimerInfo myTimer,
        [DurableClient] DurableTaskClient client)
    {
        const string instanceId = nameof(EventCityWonderRankingOrchestrator);
        var existingInstance = await client.GetInstanceAsync(instanceId);
        
        if (existingInstance == null
            || existingInstance.RuntimeStatus == OrchestrationRuntimeStatus.Completed
            || existingInstance.RuntimeStatus == OrchestrationRuntimeStatus.Failed
            || existingInstance.RuntimeStatus == OrchestrationRuntimeStatus.Terminated)
        {
            var id = await client.ScheduleNewOrchestrationInstanceAsync(nameof(EventCityWonderRankingOrchestrator),
                new StartOrchestrationOptions(instanceId));
            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", id);
        }
    }
}
