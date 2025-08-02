using Ingweland.Fog.Functions.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;

namespace Ingweland.Fog.Functions.Functions;

public class TopAllianceMembersUpdaterTrigger(ITopAllianceMembersUpdateManager topAllianceMembersUpdateManager)
{
    [Function("TopAllianceMembersUpdaterTrigger")]
    public async Task Run([TimerTrigger("0 10/10 0 * * *")] TimerInfo myTimer)
    {
        await topAllianceMembersUpdateManager.RunAsync();
    }
}
