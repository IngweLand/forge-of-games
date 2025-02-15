namespace Ingweland.Fog.Models.Hoh.Entities.Ranking;

public class AllianceRank
{
    public int AvatarBackgroundId { get; init; }
    public int AvatarIconId { get; init; }
    public string? Description { get; set; }
    public int Id { get; init; }
    public required string Language { get; set; }
    public int MaxMembers { get; set; }
    public int MemberCount { get; set; }
    public required string Name { get; init; }
    public int Points { get; init; }
    public int Rank { get; init; }
}