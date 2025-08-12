namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class AllianceDto
{
    public int AvatarBackgroundId { get; set; }
    public int AvatarIconId { get; set; }
    public required int Id { get; init; }
    public bool IsDeleted { get; init; }
    public required string Name { get; set; }
    public int Rank { get; set; }
    public int RankingPoints { get; set; }
    public required DateOnly UpdatedAt { get; set; }
    public required string WorldId { get; init; }
}
