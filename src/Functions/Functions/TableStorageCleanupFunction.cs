using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class TableStorageCleanupFunction(
    IInGameRawDataTableRepository inGameRawDataTableRepository,
    IInGameStartupDataRepository inGameStartupDataRepository,
    ILogger<TableStorageCleanupFunction> logger)
{
    [Function("TableStorageCleanupFunction")]
    public async Task Run([TimerTrigger("0 0 8 * * *")] TimerInfo myTimer)
    {
        var cutOffDate = DateTime.UtcNow.AddDays(-3);
        await inGameRawDataTableRepository.DeleteAllAsync(cutOffDate);
        logger.LogInformation("Deleted all raw data tables older than {cutOffDate}", cutOffDate);
        cutOffDate = DateTime.UtcNow.AddDays(-1);
        await inGameStartupDataRepository.DeleteAllAsync(cutOffDate);
        logger.LogInformation("Deleted all startup data tables older than {cutOffDate}", cutOffDate);
    }
}
