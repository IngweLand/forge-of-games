using Ingweland.Fog.Functions.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class TopAllianceMemberProfilesUpdaterTrigger(
    ITopAllianceMemberProfilesUpdateManager topAllianceMemberProfilesUpdateManager,
    ILogger<TopAllianceMemberProfilesUpdaterTrigger> logger)
{
    [Function(nameof(TopAllianceMemberProfilesUpdaterTrigger))]
    public Task<bool> Run([ActivityTrigger] object? _)
    {
        logger.LogInformation("{activity} started.", nameof(TopAllianceMemberProfilesUpdaterTrigger));
        return topAllianceMemberProfilesUpdateManager.RunAsync();
    }
}
