using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

public record BattleSearchRequest()
{
    public BattleType BattleType { get; init; } = BattleType.Campaign;
    public RegionId CampaignRegion { get; init; } = RegionId.DesertDelta_1;
    public int CampaignRegionEncounter { get; init; } = 1;
    public Difficulty Difficulty { get; init; } = Difficulty.Normal;
    public int TreasureHuntDifficulty { get; set; }
    public int TreasureHuntEncounter { get; set; }
    public int TreasureHuntStage { get; set; }
    public IReadOnlyCollection<string> UnitIds { get; init; } = new List<string>();
}
