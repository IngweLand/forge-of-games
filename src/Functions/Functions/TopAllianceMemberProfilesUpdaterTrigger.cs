using Ingweland.Fog.Functions.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;

namespace Ingweland.Fog.Functions.Functions;

public class TopAllianceMemberProfilesUpdaterTrigger(ITopAllianceMemberProfilesUpdateManager topAllianceMemberProfilesUpdateManager)
{
    [Function("TopAllianceMemberProfilesUpdaterTrigger")]
    public async Task Run([TimerTrigger("0 10/10 0 * * *")] TimerInfo myTimer)
    {
        await topAllianceMemberProfilesUpdateManager.RunAsync();
    }
}
