using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class AllianceMemberDto
{
    public required string Age { get; set; }
    public int AvatarId { get; set; }
    public required int PlayerId { get; init; }
    public required string Name { get; set; }
    public int RankingPoints { get; set; }
    public required DateOnly UpdatedAt { get; set; }
    public AllianceMemberRole Role { get; init; }
    public DateTime? JoinedAt { get; init; }
}
