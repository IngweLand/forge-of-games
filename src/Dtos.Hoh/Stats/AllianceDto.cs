using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class AllianceDto
{
    public int AvatarIconId { get; set; }
    public int AvatarBackgroundId { get; set; }
    public required AllianceKey Key { get; init; }
    public required string Name { get; set; }
    public int Rank { get; set; }
    public int MemberCount { get; set; }
    public int RankingPoints { get; set; }
    public required DateOnly UpdatedAt { get; set; }
}