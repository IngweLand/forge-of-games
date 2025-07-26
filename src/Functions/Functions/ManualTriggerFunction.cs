using Microsoft.Azure.Functions.Worker;

namespace Ingweland.Fog.Functions.Functions;

public class ManualTriggerFunction
{
    [Function("ManualTriggerFunction")]
    public async Task Run([TimerTrigger("0 0 0 1 1 1", RunOnStartup = false)] TimerInfo myTimer)
    {
    }
}
