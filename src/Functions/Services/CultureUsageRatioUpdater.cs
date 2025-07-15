using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.PlayerCity.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Functions.Services;

public interface ICultureUsageRatioUpdater
{
    Task RunAsync();
}

public class CultureUsageRatioUpdater(
    IFogDbContext context,
    ICityStatsCalculator cityStatsCalculator,
    IHohCityCreationService cityCreationService) : ICultureUsageRatioUpdater
{
    public async Task RunAsync()
    {
        var snapshots = await context.PlayerCitySnapshots.Where(x => x.HappinessUsageRatio == 0).ToListAsync();

        foreach (var snapshot in snapshots)
        {
            var city = await cityCreationService.Create(snapshot, string.Empty);
            var cityStats = await cityStatsCalculator.Calculate(city);
            snapshot.HappinessUsageRatio = cityStats.HappinessUsageRatio;
            snapshot.TotalArea = cityStats.TotalArea;
        }

        await context.SaveChangesAsync();
    }
}
