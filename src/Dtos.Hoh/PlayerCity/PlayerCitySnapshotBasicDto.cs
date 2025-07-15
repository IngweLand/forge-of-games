using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh.PlayerCity;

public class PlayerCitySnapshotBasicDto
{
    public required string AgeId { get; init; }
    public required CityId CityId { get; init; }
    public int Coins { get; init; }
    public int Food { get; init; }
    public int Goods { get; init; }
    public required float HappinessUsageRatio { get; init; }
    public bool HasPremiumBuildings { get; init; }
    public int Id { get; init; }
    public required string PlayerName { get; init; }
    public required int TotalArea { get; init; }
}
