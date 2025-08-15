using Ingweland.Fog.Functions.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class TopAllianceMembersUpdaterTrigger(
    ITopAllianceMemberUpdateManager topAllianceMemberUpdateManager,
    ILogger<TopHeroInsightsProcessorTrigger> logger)
{
    [Function(nameof(TopAllianceMembersUpdaterTrigger))]
    public Task<bool> Run([ActivityTrigger] object? _)
    {
        logger.LogInformation("{activity} started.", nameof(TopAllianceMembersUpdaterTrigger));
        return topAllianceMemberUpdateManager.RunAsync();
    }
}
