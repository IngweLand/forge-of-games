using Ingweland.Fog.Functions.Functions.Orchestration;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class ProcessingOrchestratorStarter(ILogger<ProcessingOrchestratorStarter> logger)
{
    [Function("ProcessingOrchestratorStarter")]
    public async Task Run([TimerTrigger("1 0 0 * * *", RunOnStartup = false)] TimerInfo myTimer,
        [DurableClient] DurableTaskClient client)
    {
        const string instanceId = nameof(NightlyProcessingOrchestrator);
        var existingInstance = await client.GetInstanceAsync(instanceId);
        
        if (existingInstance == null
            || existingInstance.RuntimeStatus == OrchestrationRuntimeStatus.Completed
            || existingInstance.RuntimeStatus == OrchestrationRuntimeStatus.Failed
            || existingInstance.RuntimeStatus == OrchestrationRuntimeStatus.Terminated)
        {
            var id = await client.ScheduleNewOrchestrationInstanceAsync(nameof(NightlyProcessingOrchestrator),
                new StartOrchestrationOptions(instanceId));
            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", id);
        }
    }
}
