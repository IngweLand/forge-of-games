using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions.Orchestration;

public abstract class SubOrchestratorBase
{
    protected abstract IReadOnlyDictionary<string, (int MaxRuns, int? CutOffHour, int[]? OnDays, int[]? NotOnDays)>
        Activities { get; }

    protected async Task DoRunOrchestrator(TaskOrchestrationContext context)
    {
        var logger = context.CreateReplaySafeLogger<FetchAndSaveOrchestrator>();

        foreach (var kvp in Activities)
        {
            var (maxRuns, cutOffHour, onDays, notOnDays) = kvp.Value;
            if (onDays == null && notOnDays == null)
            {
                await RunAsync(kvp.Key, maxRuns, context, logger, cutOffHour);
            }
            else
            {
                var orchestrationTime = context.CurrentUtcDateTime;
                if ((onDays != null && onDays.Contains(orchestrationTime.Day)) ||
                    (notOnDays != null && !notOnDays.Contains(orchestrationTime.Day)))
                {
                    await RunAsync(kvp.Key, maxRuns, context, logger, cutOffHour);
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
                    TaskOptions.FromRetryPolicy(new RetryPolicy(2, TimeSpan.FromMinutes(1))));

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
        return DateTime.UtcNow.Hour > input;
    }
}
