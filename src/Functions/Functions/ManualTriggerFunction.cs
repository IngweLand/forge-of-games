using Ingweland.Fog.Functions.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;

namespace Ingweland.Fog.Functions.Functions;

public class ManualTriggerFunction(IPlayerCityFetcher fetcher)
{
    [Function("ManualTriggerFunction")]
    public async Task Run([TimerTrigger("0 0 0 1 1 1", RunOnStartup = true)] TimerInfo myTimer)
    {
        await fetcher.RunAsync();
    }
}
