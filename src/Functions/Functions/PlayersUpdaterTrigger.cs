using Ingweland.Fog.Functions.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;

namespace Ingweland.Fog.Functions.Functions;

public class PlayersUpdaterTrigger(IPlayersUpdateManager playersUpdateManager)
{
    [Function("PlayersUpdaterTrigger")]
    public async Task Run([TimerTrigger("0 5/10 2-5 * * *")] TimerInfo myTimer)
    {
        await playersUpdateManager.RunAsync();
    }
}
