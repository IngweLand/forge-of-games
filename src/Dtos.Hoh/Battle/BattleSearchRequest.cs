using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

public record BattleSearchRequest
{
    public BattleType BattleType { get; init; } = BattleType.Campaign;
    public RegionId CampaignRegion { get; init; } = RegionId.DesertDelta_1;
    public int CampaignRegionEncounter { get; init; } = 1;
    public Difficulty Difficulty { get; init; } = Difficulty.Normal;
    public RegionId HistoricBattleRegion { get; init; } = RegionId.SiegeOfOrleans;
    public int HistoricBattleEncounter { get; init; } = 1;
    public int TreasureHuntDifficulty { get; init; }
    public int TreasureHuntEncounter { get; init; }
    public int TreasureHuntStage { get; init; }
    public IReadOnlyCollection<string> UnitIds { get; init; } = new List<string>();
}
