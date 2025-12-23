using Ingweland.Fog.Functions.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions.Orchestration;

public abstract class SubOrchestratorBase
{
    protected abstract IReadOnlyDictionary<string, ActivityConfiguration> Activities { get; }

    protected async Task DoRunOrchestrator(TaskOrchestrationContext context)
    {
        var logger = context.CreateReplaySafeLogger<FetchAndSaveOrchestrator>();

        foreach (var kvp in Activities)
        {
            var config = kvp.Value;
            if (config.OnDays == null && config.NotOnDays == null)
            {
                await RunAsync(kvp.Key, config.MaxRuns, context, logger, config.CutOffHour);
            }
            else
            {
                var orchestrationTime = context.CurrentUtcDateTime;
                if ((config.OnDays != null && config.OnDays.Contains(orchestrationTime.Day)) ||
                    (config.NotOnDays != null && !config.NotOnDays.Contains(orchestrationTime.Day)))
                {
                    await RunAsync(kvp.Key, config.MaxRuns, context, logger, config.CutOffHour);
                }
            }
        }
    }

    private async Task RunAsync(string activityName, int maxRuns, TaskOrchestrationContext context, ILogger logger,
        int? cutOffHour = null)
    {
        logger.LogInformation("Running {f}.", activityName);
        context.SetCustomStatus($"Running {activityName}");
        try
        {
            for (var i = 0; i < maxRuns; i++)
            {
                var skipDueToHour = cutOffHour.HasValue &&
                    await context.CallActivityAsync<bool>(nameof(CheckIfAfterHour), cutOffHour);

                if (skipDueToHour)
                {
                    logger.LogInformation("Stopping {f} after {i} runs due to time cutoff.", activityName, i);
                    break;
                }

                logger.LogInformation("Running {f}. Run: {i}", activityName, i + 1);
                var shouldRunAgain = await context.CallActivityAsync<bool>(activityName, i,
                    TaskOptions.FromRetryPolicy(new RetryPolicy(2, TimeSpan.FromSeconds(10))));

                if (!shouldRunAgain)
                {
                    break;
                }
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Activity {f} failed, continuing with next", activityName);
        }
    }

    [Function(nameof(CheckIfAfterHour))]
    public static bool CheckIfAfterHour([ActivityTrigger] int input)
    {
        const int functionMaxRunTimeMinutes = 10;
        return DateTime.UtcNow.AddMinutes(functionMaxRunTimeMinutes).Hour > input;
    }
}
