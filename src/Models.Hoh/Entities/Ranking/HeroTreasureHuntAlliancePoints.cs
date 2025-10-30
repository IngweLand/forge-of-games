using Ingweland.Fog.Models.Hoh.Entities.Alliance;

namespace Ingweland.Fog.Models.Hoh.Entities.Ranking;

public class HeroTreasureHuntAlliancePoints
{
    public required HohAlliance Alliance { get; init; }
    public required int EventId { get; init; }
    public required int League { get; init; }
    public required int Points { get; init; }
    public required DateTime UpdatedAt { get; init; }
}
