namespace Ingweland.Fog.Application.Core.CityPlanner.Stats.BuildingTypedStats;

public class ProductionCostStatsItem
{
    public required int Default { get; init; }
    public required int OneHour { get; init; }
    public required int OneDay { get; init; }
    public required string ResourceId { get; init; }
}
