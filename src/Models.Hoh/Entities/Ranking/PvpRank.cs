namespace Ingweland.Fog.Models.Hoh.Entities.Ranking;

public class PvpRank
{
    public required int Rank { get; init; }
    public required int Points { get; init; }
    public required HohPlayer Player { get; init; }
    public HohAlliance? Alliance { get; init; }
}