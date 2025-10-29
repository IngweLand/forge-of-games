using Ingweland.Fog.Functions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class RankingsCleanupTrigger(
    IRankingsCleanupService rankingsCleanupService,
    ILogger<RankingsCleanupTrigger> logger)
{
    [Function("RankingsCleanupTrigger")]
    public async Task Run([TimerTrigger("0 0 0 */1 * *")] TimerInfo myTimer)
    {
        await rankingsCleanupService.RunAsync();
    }
}
