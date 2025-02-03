namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats.BuildingTypedStats;

public class ProductionStatsItem
{
    public required IReadOnlyCollection<ProductStatsItem> Products { get; init; } = new List<ProductStatsItem>();
    public int WorkerCount { get; init; }
    public required string ProductionId { get; init; }
}
