using System.Collections.ObjectModel;
using Ingweland.Fog.Functions.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;

namespace Ingweland.Fog.Functions.Functions.Orchestration;

public class EventCityWonderRankingOrchestrator : SubOrchestratorBase
{
    protected override IReadOnlyDictionary<string, ActivityConfiguration> Activities =>
        new ReadOnlyDictionary<string, ActivityConfiguration>(new Dictionary<string, ActivityConfiguration>
        {
            {nameof(EventCityWonderRankingTrigger), new ActivityConfiguration(7, 12)},
        });

    [Function(nameof(EventCityWonderRankingOrchestrator))]
    public async Task RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
    {
        await DoRunOrchestrator(context);
    }
}
