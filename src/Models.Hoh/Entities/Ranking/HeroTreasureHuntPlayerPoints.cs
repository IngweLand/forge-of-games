namespace Ingweland.Fog.Models.Hoh.Entities.Ranking;

public class HeroTreasureHuntPlayerPoints
{
    public required int Difficulty { get; init; }
    public required HohPlayer Player { get; init; }
    public required int RankingPoints { get; init; }
    public required int TreasureHuntEventId { get; init; }
    public required DateTime UpdatedAt { get; init; }
}
