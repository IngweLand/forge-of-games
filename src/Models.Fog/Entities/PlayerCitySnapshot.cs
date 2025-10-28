using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class PlayerCitySnapshot
{
    public required string AgeId { get; set; }
    public required CityId CityId { get; set; }
    public int Coins { get; set; }

    public required DateOnly CollectedAt { get; set; }

    public required PlayerCitySnapshotDataEntity Data { get; set; }

    public int Food { get; set; }
    public int Goods { get; set; }

    public float HappinessUsageRatio { get; set; }
    public bool HasPremiumBuildings { get; set; }
    public int Id { get; set; }
    public required string OpenedExpansionsHash { get; set; }
    public Player Player { get; set; } = null!;

    public int PlayerId { get; set; }
    public required int TotalArea { get; set; }
}
