using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh.PlayerCity;

public record CityInspirationsSearchRequest
{
    public required string AgeId { get; init; }
    public bool AllowPremiumCultureBuildings { get; init; }
    public bool AllowPremiumFarmBuildings { get; init; }
    public bool AllowPremiumHomeBuildings { get; init; }
    public required CityId CityId { get; init; }
    public string? OpenedExpansionsHash { get; init; }
    public CityProductionMetric ProductionMetric { get; init; } = CityProductionMetric.Storage;
    public CitySnapshotSearchPreference SearchPreference { get; init; } = CitySnapshotSearchPreference.Food;
    public int TotalArea { get; init; }
}
