namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class ProductionStatsViewModel
{
    public required IReadOnlyCollection<TimedProductionValuesViewModel> Products { get; init; }
    public required IReadOnlyCollection<TimedProductionValuesViewModel> Costs { get; init; }
}
