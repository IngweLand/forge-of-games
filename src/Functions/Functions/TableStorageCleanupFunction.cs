using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Microsoft.Azure.Functions.Worker;

namespace Ingweland.Fog.Functions.Functions;

public class TableStorageCleanupFunction(
    IInGameRawDataTableRepository inGameRawDataTableRepository,
    IInGameStartupDataRepository inGameStartupDataRepository)
{
    [Function("TableStorageCleanupFunction")]
    public async Task Run([TimerTrigger("0 0 8 * * *")] TimerInfo myTimer)
    {
        var cutOffDate = DateTime.UtcNow.AddDays(-7);
        await inGameRawDataTableRepository.DeleteAllAsync(cutOffDate);
        cutOffDate = DateTime.UtcNow.AddDays(-1);
        await inGameStartupDataRepository.DeleteAllAsync(cutOffDate);
    }
}
