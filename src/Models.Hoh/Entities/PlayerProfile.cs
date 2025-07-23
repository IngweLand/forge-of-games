namespace Ingweland.Fog.Models.Hoh.Entities;

public class PlayerProfile
{
    public HohAlliance? Alliance { get; init; }
    public required HohPlayer Player { get; init; }
    public string? PvpTier { get; set; }
    public required int Rank { get; init; }
    public required int RankingPoints { get; init; }
    public IReadOnlyCollection<ProfileSquad> Squads { get; set; } = [];
    public int? TreasureHuntDifficulty { get; set; }
}
