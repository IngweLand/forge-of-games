namespace Ingweland.Fog.Models.Hoh.Entities;

public class AllianceMember
{
    public required HohPlayer Player { get; init; }
    public required int RankingPoints { get; init; }
}