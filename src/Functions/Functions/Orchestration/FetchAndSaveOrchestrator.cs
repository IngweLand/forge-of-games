using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions.Orchestration;

public class FetchAndSaveOrchestrator
{
    private static readonly Dictionary<string, int> MaxRuns = new()
    {
        {nameof(TopAllianceMembersUpdaterTrigger), 12},
        {nameof(AllianceMembersUpdaterTrigger), 5},
        {nameof(TopAllianceMemberProfilesUpdaterTrigger), 10},
        {nameof(TopPlayersUpdaterTrigger), 10},
        {nameof(PlayersUpdaterTrigger), 30},
        {nameof(PlayerCityFetcherTrigger), 5},
        {nameof(TopPlayersCityFetcherTrigger), 6},
    };

    private ILogger _logger;

    [Function(nameof(FetchAndSaveOrchestrator))]
    public async Task RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
    {
        _logger = context.CreateReplaySafeLogger<FetchAndSaveOrchestrator>();

        await RunAsync(nameof(TopAllianceMembersUpdaterTrigger), context, 7);
        await RunAsync(nameof(AllianceMembersUpdaterTrigger), context, 7);
        await RunAsync(nameof(TopAllianceMemberProfilesUpdaterTrigger), context, 7);
        await RunAsync(nameof(TopPlayersUpdaterTrigger), context, 7);
        await RunAsync(nameof(PlayersUpdaterTrigger), context, 7);

        var orchestrationTime = context.CurrentUtcDateTime;
        var isSpecialDay = orchestrationTime.Day is 1 or 15;
        if (isSpecialDay)
        {
            await RunAsync(nameof(TopPlayersCityFetcherTrigger), context, 7);
        }
        else
        {
            await RunAsync(nameof(PlayerCityFetcherTrigger), context, 7);
        }
        
        await RunAsync(nameof(TopHeroInsightsProcessorTrigger), context, null);
        await RunAsync(nameof(InGameEventsFetcherTrigger), context, null);
    }

    private async Task RunAsync(string triggerFuncName, TaskOrchestrationContext context, int? cutOffHour)
    {
        _logger.LogInformation("Running {f}.", triggerFuncName);
        context.SetCustomStatus($"Running {triggerFuncName}");
        try
        {
            var maxRuns = MaxRuns.GetValueOrDefault(triggerFuncName, 1);
            for (var i = 0; i < maxRuns; i++)
            {
                var skipDueToHour = cutOffHour.HasValue &&
                    await context.CallActivityAsync<bool>(nameof(CheckIfAfterHour), cutOffHour);

                if (skipDueToHour)
                {
                    _logger.LogInformation("Stopping {f} after {i} runs due to time cutoff.", triggerFuncName, i);
                    break;
                }

                _logger.LogInformation("Running {f}. Run: {i}", triggerFuncName, i + 1);
                var shouldRunAgain = await context.CallActivityAsync<bool>(triggerFuncName,
                    TaskOptions.FromRetryPolicy(new RetryPolicy(2, TimeSpan.FromMinutes(1))));

                if (!shouldRunAgain)
                {
                    break;
                }

                _logger.LogInformation("Scheduling run {i} for {f}", i + 2, triggerFuncName);
                await context.CreateTimer(TimeSpan.FromMinutes(3), CancellationToken.None);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Activity {f} failed, continuing with next", triggerFuncName);
        }
    }

    [Function(nameof(CheckIfAfterHour))]
    public static bool CheckIfAfterHour([ActivityTrigger] int input)
    {
        return DateTime.UtcNow.Hour > input;
    }
}
