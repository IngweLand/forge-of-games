using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class TableStorageCleanupFunction(
    IInGameRawDataTableRepository inGameRawDataTableRepository,
    ILogger<TableStorageCleanupFunction> logger)
{
    [Function("TableStorageCleanupFunction")]
    public async Task Run([TimerTrigger("0 0 0 * * *")] TimerInfo myTimer)
    {
        var cutOffDate = DateTime.UtcNow.AddMonths(-1);
        await inGameRawDataTableRepository.DeleteAllAsync(cutOffDate);
    }
}
