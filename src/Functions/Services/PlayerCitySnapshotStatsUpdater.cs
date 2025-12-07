using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.PlayerCity.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services;

public interface IPlayerCitySnapshotStatsUpdater
{
    Task RunAsync();
}

public class PlayerCitySnapshotStatsUpdater(
    IFogDbContext context,
    IPlayerCityService playerCityService,
    ILogger<PlayerCitySnapshotStatsUpdater> logger) : IPlayerCitySnapshotStatsUpdater
{
    private const int BATCH_SIZE = 1000;

    public async Task RunAsync()
    {
        var i = 0;
        while (true)
        {
            var snapshotIds = await context.PlayerCitySnapshots.OrderBy(x => x.Id).Select(x => x.Id)
                .Skip(i * BATCH_SIZE).Take(BATCH_SIZE)
                .ToListAsync();

            if (snapshotIds.Count == 0)
            {
                break;
            }

            await playerCityService.RecalculateStatsAsync(snapshotIds);
            i++;
            logger.LogInformation("Processed {t}", i * BATCH_SIZE);
        }
        
        logger.LogInformation("DONE");
    }
}
