using Ingweland.Fog.Models.Hoh.Entities.Ranking;

namespace Ingweland.Fog.Models.Hoh.Entities;

public class Wakeup
{
    public AllianceWithMembers? AllianceWithMembers { get; init; } 

    public HohAlliance? Alliance { get; set; }

    public required IReadOnlyCollection<HeroTreasureHuntAlliancePoints> AthAllianceRankings { get; init; } =
        Array.Empty<HeroTreasureHuntAlliancePoints>();

    public required IReadOnlyCollection<HeroTreasureHuntPlayerPoints> AthPlayerRankings { get; init; } =
        Array.Empty<HeroTreasureHuntPlayerPoints>();
}