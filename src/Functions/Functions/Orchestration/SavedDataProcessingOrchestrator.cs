using System.Collections.ObjectModel;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;

namespace Ingweland.Fog.Functions.Functions.Orchestration;

public class SavedDataProcessingOrchestrator : SubOrchestratorBase
{
    protected override IReadOnlyDictionary<string, (int MaxRuns, int? CutOffHour, int[]? OnDays, int[]? NotOnDays)>
        Activities => new ReadOnlyDictionary<string, (int MaxRuns, int? CutOffHour, int[]? OnDays, int[]? NotOnDays)>(
        new Dictionary<string, (int MaxRuns, int? CutOffHour, int[]? OnDays, int[]? NotOnDays)>
        {
            {nameof(PlayerDataProcessor), (5, null, null, null)},
            {nameof(AllianceDataProcessor), (5, null, null, null)},
            {nameof(BattlesProcessor), (1, null, null, null)},
        });

    [Function(nameof(SavedDataProcessingOrchestrator))]
    public async Task RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
    {
        await DoRunOrchestrator(context);
    }
}
