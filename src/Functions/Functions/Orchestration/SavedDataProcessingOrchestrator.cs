using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions.Orchestration;

public class SavedDataProcessingOrchestrator(ILogger<SavedDataProcessingOrchestrator> logger)
{
    [Function(nameof(SavedDataProcessingOrchestrator))]
    public async Task RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var logger = context.CreateReplaySafeLogger<SavedDataProcessingOrchestrator>();
        try
        {
            logger.LogInformation("Running {activity}", nameof(PlayerDataProcessor));
            context.SetCustomStatus($"Running {nameof(PlayerDataProcessor)}");
            await context.CallActivityAsync<string>(nameof(PlayerDataProcessor),
                TaskOptions.FromRetryPolicy(new RetryPolicy(2, TimeSpan.FromSeconds(1))));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Activity {f} failed.", nameof(PlayerDataProcessor));
        }
        
        try
        {
            logger.LogInformation("Running {activity}", nameof(AllianceDataProcessor));
            context.SetCustomStatus($"Running {nameof(AllianceDataProcessor)}");
            await context.CallActivityAsync<string>(nameof(AllianceDataProcessor),
                TaskOptions.FromRetryPolicy(new RetryPolicy(2, TimeSpan.FromSeconds(1))));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Activity {f} failed.", nameof(AllianceDataProcessor));
        }
        
        try
        {
            logger.LogInformation("Running {activity}", nameof(BattlesProcessor));
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
