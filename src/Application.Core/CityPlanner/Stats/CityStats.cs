using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.CityPlanner.Stats;

public class CityStats
{
    public int TotalArea { get; set; }
    public IDictionary<BuildingGroup, int> AreasByGroup { get; } = new Dictionary<BuildingGroup, int>();
    public IDictionary<BuildingType, int> AreasByType { get; } = new Dictionary<BuildingType, int>();
    public int ExcessHappiness { get; set; }

    public float HappinessUsageRatio
    {
        get
        {
            if (TotalAvailableHappiness <= 0 || TotalHappinessConsumption <= 0)
            {
                return 0;
            }
            return MathF.Round((float) TotalHappinessConsumption / TotalAvailableHappiness, 2,
                MidpointRounding.AwayFromZero);
        }
    }

    public IDictionary<string, ConsolidatedCityProduct> Products { get; } =
        new Dictionary<string, ConsolidatedCityProduct>();

    public int ProvidedWorkersCount { get; set; }

    public int RequiredWorkersCount { get; set; }
    public int TotalAvailableHappiness { get; set; }
    public int TotalHappinessConsumption { get; set; }
    public int UnmetHappinessNeed { get; set; }
}
