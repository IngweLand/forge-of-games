using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.CityPlanner.Stats;

public class CityStats
{
    public IDictionary<BuildingGroup, (int Count, int Area)> AreasByGroup { get; } =
        new Dictionary<BuildingGroup, (int Count, int Area)>();

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

    public int PremiumExpansionCount { get; set; }

    public IDictionary<string, ConsolidatedTimedProductionValues> ProductionCosts { get; } =
        new Dictionary<string, ConsolidatedTimedProductionValues>();

    public IDictionary<string, ConsolidatedTimedProductionValues> Products { get; } =
        new Dictionary<string, ConsolidatedTimedProductionValues>();

    public int ProvidedWorkersCount { get; set; }

    public int RequiredWorkersCount { get; set; }
    public int TotalArea { get; set; }
    public int TotalAvailableHappiness { get; set; }
    public int TotalHappinessConsumption { get; set; }
    public int UnmetHappinessNeed { get; set; }

    public IReadOnlyDictionary<string, double>? WonderResourcesBonus { get; set; }
    public int WonderWorkersBonus { get; set; }
}
