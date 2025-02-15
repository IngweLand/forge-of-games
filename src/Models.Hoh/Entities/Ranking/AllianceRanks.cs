using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Hoh.Entities.Ranking;

public class AllianceRanks
{
    public required IReadOnlyCollection<AllianceRank> SurroundingRanking { get; init; } = new List<AllianceRank>();
    public required IReadOnlyCollection<AllianceRank> Top100 { get; init; } = new List<AllianceRank>();
    public AllianceRankingType Type { get; init; }
}