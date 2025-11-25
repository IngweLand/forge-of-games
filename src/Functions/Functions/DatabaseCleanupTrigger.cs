using Ingweland.Fog.Functions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class DatabaseCleanupTrigger(
    IDatabaseCleanupService databaseCleanupService,
    ILogger<DatabaseCleanupTrigger> logger)
{
    [Function("DatabaseCleanupTrigger")]
    public async Task Run([TimerTrigger("0 0 8 */1 * *")] TimerInfo myTimer)
    {
        await databaseCleanupService.RunAsync();
    }
}
