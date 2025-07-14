using Ingweland.Fog.Functions.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;

namespace Ingweland.Fog.Functions.Functions;

public class TopPlayersUpdaterTrigger(ITopPlayersUpdateManager topPlayersUpdateManager)
{
    [Function("TopPlayersUpdaterTrigger")]
    public async Task Run([TimerTrigger("0 30,37,44,51 0 * * *")] TimerInfo myTimer)
    {
        await topPlayersUpdateManager.RunAsync();
    }
}
