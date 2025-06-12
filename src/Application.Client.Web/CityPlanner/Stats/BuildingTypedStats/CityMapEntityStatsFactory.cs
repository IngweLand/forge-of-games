using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats.BuildingTypedStats;

public class CityMapEntityStatsFactory : ICityMapEntityStatsFactory
{
    public IReadOnlyCollection<ICityMapEntityStats> Create(BuildingDto building)
    {
        var stats = new List<ICityMapEntityStats>();

        stats.Add(new AreaProvider()
        {
            BuildingType = building.Type,
            BuildingGroup = building.Group,
            Area = building.Width * building.Length,
        });

        if (building.BuffDetails != null)
        {
            stats.Add(new HappinessConsumer()
            {
                BuffDetails = building.BuffDetails,
            });
        }

        var productionComponents = building.Components.OfType<ProductionComponent>().ToList();
        if (productionComponents.Count > 0)
        {
            stats.Add(new ProductionProvider()
            {
                ProductionComponents = productionComponents,
            });
        }

        if (building.CultureComponent != null)
        {
            stats.Add(new HappinessProvider()
            {
                CultureComponent = building.CultureComponent,
                Range = building.CultureComponent.Range ?? 0,
                Value = building.CultureComponent.Value ?? 0,
            });
        }

        var grantWorkerComponents = building.Components.OfType<GrantWorkerComponent>().ToList();
        if (grantWorkerComponents.Count > 0)
        {
            stats.Add(new WorkerProvider()
            {
                WorkerCount = grantWorkerComponents.Sum(src => src.GetWorkerCount()),
            });
        }

        return stats;
    }
}
