using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Hoh.Entities.Ranking;

public class PlayerRanks
{
    public required IReadOnlyCollection<PlayerRank> SurroundingRanking { get; init; } = new List<PlayerRank>();
    public required IReadOnlyCollection<PlayerRank> Top100 { get; init; } = new List<PlayerRank>();
    public PlayerRankingType Type { get; init; }
}
