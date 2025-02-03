namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats.BuildingTypedStats;

public class ProductStatsItem
{
    public required TimedProductStatsItem DefaultProduction { get; init; }
    public required TimedProductStatsItem OneHourProduction { get; init; }
    public required TimedProductStatsItem OneDayProduction { get; init; }
    public required string ResourceId { get; init; }
}
