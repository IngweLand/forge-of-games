using Ingweland.Fog.Functions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class DatabaseCleanupTrigger(IDatabaseCleanupService databaseCleanupService)
{
    [Function("DatabaseCleanupTrigger")]
    public async Task Run([TimerTrigger("0 0 8 * * *")] TimerInfo myTimer)
    {
        await databaseCleanupService.RunAsync();
    }
}
