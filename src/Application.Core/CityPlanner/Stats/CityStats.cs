using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.CityPlanner.Stats;

public class CityStats
{
    public int ExcessHappiness { get; set; }

    public float HappinessUsageRatio =>
        MathF.Round((float) TotalHappinessConsumption / TotalAvailableHappiness, 2, MidpointRounding.AwayFromZero);

    public IDictionary<string, ConsolidatedCityProduct> Products { get; } =
        new Dictionary<string, ConsolidatedCityProduct>();

    public int ProvidedWorkersCount { get; set; }

    public int RequiredWorkersCount { get; set; }
    public int TotalAvailableHappiness { get; set; }
    public int TotalHappinessConsumption { get; set; }
    public int UnmetHappinessNeed { get; set; }
    public IDictionary<BuildingType, int> AreasByType { get; } = new Dictionary<BuildingType, int>();
    public IDictionary<BuildingGroup, int> AreasByGroup { get; } = new Dictionary<BuildingGroup, int>();
}
