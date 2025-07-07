using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh.PlayerCity;

public record CityInspirationsSearchRequest
{
    public required string AgeId { get; init; } 
    public bool AllowPremiumEntities { get; set; }
    public required CityId CityId { get; init; }
    public string? OpenedExpansionsHash { get; init; }
    public CitySnapshotSearchPreference SearchPreference { get; init; } = CitySnapshotSearchPreference.Food;
}
