using Ingweland.Fog.Functions.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;

namespace Ingweland.Fog.Functions.Functions;

public class PlayerCityFetcherTrigger(IPlayerCityFetcher playerCityFetcher)
{
    [Function("PlayerCityFetcherTrigger")]
    public async Task Run([TimerTrigger("0 10/15 1-5 * * *")] TimerInfo myTimer)
    {
        await playerCityFetcher.RunAsync();
    }
}
