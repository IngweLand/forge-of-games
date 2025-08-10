using Ingweland.Fog.Models.Hoh.Entities.Alliance;

namespace Ingweland.Fog.Models.Hoh.Entities.Ranking;

public class HeroTreasureHuntAlliancePoints
{
    public required int TreasureHuntEventId { get; init; }
    public required HohAlliance Alliance { get; init; }
    public required int RankingPoints { get; init; }
}