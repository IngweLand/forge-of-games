using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;

namespace Ingweland.Fog.Functions.Functions.Orchestration;

public class NightlyProcessingOrchestrator
{
    [Function(nameof(NightlyProcessingOrchestrator))]
    public async Task RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
    {
        await context.CallSubOrchestratorAsync(nameof(SavedDataProcessingOrchestrator));
        await context.CallSubOrchestratorAsync(nameof(FetchAndSaveOrchestrator));
    }
}
