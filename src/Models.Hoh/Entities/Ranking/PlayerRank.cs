namespace Ingweland.Fog.Models.Hoh.Entities.Ranking;

public class PlayerRank
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public int AvatarId { get; init; }
    public required string Age { get; init; }
    public string? AllianceName { get; init; }
    public int Rank { get; init; }
    public int Points { get; init; }
}
