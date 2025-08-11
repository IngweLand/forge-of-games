using System;
using Ingweland.Fog.Functions.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class TopAllianceMembersUpdaterTrigger(ITopAllianceMemberUpdateManager topAllianceMemberUpdateManager)
{
    [Function("TopAllianceMembersUpdaterTrigger")]
    public async Task Run([TimerTrigger("0 10/10 0 * * *")] TimerInfo myTimer)
    {
        await topAllianceMemberUpdateManager.RunAsync();
    }
}
