using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions.Orchestration;

public class SavedDataProcessingOrchestrator()
{
    [Function(nameof(SavedDataProcessingOrchestrator))]
    public async Task RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var logger = context.CreateReplaySafeLogger<FetchAndSaveOrchestrator>();
        try
        {
            context.SetCustomStatus($"Running {nameof(AutoDataProcessor)}");
            await context.CallActivityAsync<string>(nameof(AutoDataProcessor),
                TaskOptions.FromRetryPolicy(new RetryPolicy(2, TimeSpan.FromSeconds(1))));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Activity {f} failed.", nameof(AutoDataProcessor));
        }
        
        try
        {
            context.SetCustomStatus($"Running {nameof(BattlesProcessor)}");
            await context.CallActivityAsync<string>(nameof(BattlesProcessor),
                TaskOptions.FromRetryPolicy(new RetryPolicy(2, TimeSpan.FromSeconds(1))));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Activity {f} failed.", nameof(BattlesProcessor));
        }
    }
}
