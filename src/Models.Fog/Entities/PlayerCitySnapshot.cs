using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class PlayerCitySnapshot
{
    public required string AgeId { get; set; }
    public required CityId CityId { get; set; }
    public int Coins { get; set; }
    public int Coins1H { get; set; }
    public int Coins1HPerArea { get; set; }
    public int Coins24H { get; set; }
    public int Coins24HPerArea { get; set; }
    public int CoinsPerArea { get; set; }

    public required DateOnly CollectedAt { get; set; }

    public required PlayerCitySnapshotDataEntity Data { get; set; }

    public int Food { get; set; }
    public int Food1H { get; set; }
    public int Food1HPerArea { get; set; }
    public int Food24H { get; set; }
    public int Food24HPerArea { get; set; }
    public int FoodPerArea { get; set; }

    public int Goods { get; set; }
    public int Goods1H { get; set; }
    public int Goods1HPerArea { get; set; }
    public int Goods24H { get; set; }
    public int Goods24HPerArea { get; set; }
    public int GoodsPerArea { get; set; }

    public float HappinessUsageRatio { get; set; }
    public bool HasPremiumCultureBuildings { get; set; }
    public bool HasPremiumFarmBuildings { get; set; }
    public bool HasPremiumHomeBuildings { get; set; }
    public int Id { get; set; }
    public required string OpenedExpansionsHash { get; set; }
    public Player Player { get; set; } = null!;

    public int PlayerId { get; set; }
    public int PremiumExpansionCount { get; set; }
    public required int TotalArea { get; set; }
}
