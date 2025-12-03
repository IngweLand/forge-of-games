using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class TableStorageCleanupFunction(IInGameRawDataTableRepository inGameRawDataTableRepository)
{
    [Function("TableStorageCleanupFunction")]
    public async Task Run([TimerTrigger("0 0 8 * * *")] TimerInfo myTimer)
    {
        var cutOffDate = DateTime.UtcNow.AddDays(-7);
        await inGameRawDataTableRepository.DeleteAllAsync(cutOffDate);
    }
}
