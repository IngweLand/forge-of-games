using Ingweland.Fog.Models.Hoh.Entities.Alliance;

namespace Ingweland.Fog.Models.Hoh.Entities.Ranking;

public class AllianceRank
{
    public required AllianceBanner Banner { get; init; }
    public string? Description { get; set; }
    public int Id { get; init; }
    public required string Language { get; set; }
    public required HohPlayer Leader { get; init; }
    public int MaxMembers { get; set; }
    public int MemberCount { get; set; }
    public required string Name { get; init; }
    public int Points { get; init; }
    public int Rank { get; init; }
}
