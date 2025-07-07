using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh.PlayerCity;

public class PlayerCitySnapshotBasicDto
{
    public required CityId CityId { get; init; }
    public required string AgeId { get; init; }
    public int Coins { get; init; }
    public int Food { get; init; }
    public int Goods { get; init; }
    public bool HasPremiumBuildings { get; init; }
    public int Id { get; init; }
    public required string PlayerName { get; init; }
}
