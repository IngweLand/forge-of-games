using System.Collections.ObjectModel;
using Ingweland.Fog.Functions.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;

namespace Ingweland.Fog.Functions.Functions.Orchestration;

public class FetchAndSaveOrchestrator : SubOrchestratorBase
{
    protected override IReadOnlyDictionary<string, ActivityConfiguration> Activities =>
        new ReadOnlyDictionary<string, ActivityConfiguration>(new Dictionary<string, ActivityConfiguration>
        {
            {nameof(TopAlliancesUpdaterTrigger), new ActivityConfiguration(6, 7)},
            {nameof(AlliancesUpdaterTrigger), new ActivityConfiguration(5, 7)},
            {nameof(TopAllianceMembersUpdaterTrigger), new ActivityConfiguration(12, 7)},
            {nameof(AllianceMembersUpdaterTrigger), new ActivityConfiguration(5, 7)},
            {nameof(TopAllianceMemberProfilesUpdaterTrigger), new ActivityConfiguration(10, 7)},
            {nameof(TopPlayersUpdaterTrigger), new ActivityConfiguration(10, 7)},
            {nameof(PlayersUpdaterTrigger), new ActivityConfiguration(30, 7)},
            {nameof(PlayerCityFetcherTrigger), new ActivityConfiguration(5, 7, null, [1, 15])},
            {nameof(TopPlayersCityFetcherTrigger), new ActivityConfiguration(6, 7, [1, 15])},
            {nameof(TopHeroInsightsProcessorTrigger), new ActivityConfiguration(1)},
            {nameof(InGameEventsFetcherTrigger), new ActivityConfiguration(1)},
        });

    [Function(nameof(FetchAndSaveOrchestrator))]
    public async Task RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
    {
        await DoRunOrchestrator(context);
    }
}
