using System.Collections.ObjectModel;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;

namespace Ingweland.Fog.Functions.Functions.Orchestration;

public class FetchAndSaveOrchestrator : SubOrchestratorBase
{
    protected override IReadOnlyDictionary<string, (int MaxRuns, int? CutOffHour, int[]? OnDays, int[]? NotOnDays)>
        Activities => new ReadOnlyDictionary<string, (int MaxRuns, int? CutOffHour, int[]? OnDays, int[]? NotOnDays)>(
        new Dictionary<string, (int MaxRuns, int? CutOffHour, int[]? OnDays, int[]? NotOnDays)>
        {
            {nameof(TopAlliancesUpdaterTrigger), (6, 7, null, null)},
            {nameof(AlliancesUpdaterTrigger), (5, 7, null, null)},
            {nameof(TopAllianceMembersUpdaterTrigger), (12, 7, null, null)},
            {nameof(AllianceMembersUpdaterTrigger), (5, 7, null, null)},
            {nameof(TopAllianceMemberProfilesUpdaterTrigger), (10, 7, null, null)},
            {nameof(TopPlayersUpdaterTrigger), (10, 7, null, null)},
            {nameof(PlayersUpdaterTrigger), (30, 7, null, null)},
            {nameof(PlayerCityFetcherTrigger), (5, 7, null, [1, 15])},
            {nameof(TopPlayersCityFetcherTrigger), (6, 7, [1, 15], null)},
            {nameof(TopHeroInsightsProcessorTrigger), (1, null, null, null)},
            {nameof(InGameEventsFetcherTrigger), (1, null, null, null)},
        });

    [Function(nameof(FetchAndSaveOrchestrator))]
    public async Task RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
    {
        await DoRunOrchestrator(context);
    }
}
