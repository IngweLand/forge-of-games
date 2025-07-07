namespace Ingweland.Fog.Models.Hoh.Entities;

public class PlayerProfile
{
    public required HohPlayer Player { get; init; }
    public HohAlliance? Alliance { get; init; }
    public required int Rank { get; init; }
    public required int RankingPoints { get; init; }
}
