using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class AllianceMemberDto
{
    public required string Age { get; set; }
    public int AvatarId { get; set; }
    public DateTime? JoinedAt { get; init; }
    public required string Name { get; set; }
    public required int PlayerId { get; init; }
    public int Rank { get; set; }
    public int RankingPoints { get; set; }
    public AllianceMemberRole Role { get; init; }
    public required DateOnly UpdatedAt { get; set; }
}
