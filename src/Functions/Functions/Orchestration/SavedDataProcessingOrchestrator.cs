using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions.Orchestration;

public class SavedDataProcessingOrchestrator
{
    private static readonly TimeSpan MaxDuration = TimeSpan.FromMinutes(21);

    [Function(nameof(SavedDataProcessingOrchestrator))]
    public async Task RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var logger = context.CreateReplaySafeLogger<SavedDataProcessingOrchestrator>();

        await ProcessActivityWithTimeout(
            context,
            logger,
            nameof(PlayerDataProcessor),
            MaxDuration
        );

        await ProcessActivityWithTimeout(
            context,
            logger,
            nameof(AllianceDataProcessor),
            MaxDuration
        );

        await ProcessActivityWithTimeout(
            context,
            logger,
            nameof(BattlesProcessor),
            MaxDuration
        );
    }

    private static async Task ProcessActivityWithTimeout(
        TaskOrchestrationContext context,
        ILogger logger,
        string activityName,
        TimeSpan maxDuration)
    {
        using var cts = new CancellationTokenSource();

        try
        {
            logger.LogInformation("Starting {activity}", activityName);
            context.SetCustomStatus($"Running {activityName}");

            var activityTask = context.CallActivityAsync<string>(
                activityName,
                TaskOptions.FromRetryPolicy(new RetryPolicy(2, TimeSpan.FromSeconds(1)))
            );

            var timeoutTask = context.CreateTimer(
                context.CurrentUtcDateTime.Add(maxDuration),
                cts.Token
            );

            var completedTask = await Task.WhenAny(activityTask, timeoutTask);

            if (completedTask == timeoutTask)
            {
                logger.LogError(
                    "{activity} exceeded maximum duration of {duration}. Moving to next activity.",
                    activityName,
                    maxDuration
                );
                context.SetCustomStatus($"{activityName} timed out - continuing");
            }
            else
            {
                await cts.CancelAsync();

                await activityTask;
                logger.LogInformation("{activity} completed successfully", activityName);
                context.SetCustomStatus($"{activityName} completed");
            }
        }
        catch (Exception e)
        {
            await cts.CancelAsync();

            logger.LogError(e, "{activity} failed. Moving to next activity.", activityName);
            context.SetCustomStatus($"{activityName} failed - continuing");
        }
    }
}
