using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Core.CityPlanner.Stats.BuildingTypedStats;

public class ProductionProvider : ICityMapEntityStats
{
    public required IReadOnlyCollection<ProductionComponent> ProductionComponents { get; init; }

    public IReadOnlyCollection<ProductionStatsItem> ProductionStatsItems { get; set; } =
        new List<ProductionStatsItem>();
}
