using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class PlayerDto
{
    public required string Age { get; set; }
    public string? AllianceName { get; init; }
    public int AvatarId { get; set; }
    public required PlayerKey Key { get; init; }
    public required string Name { get; set; }
    public int Rank { get; set; }
    public int RankingPoints { get; set; }
    public required DateOnly UpdatedAt { get; set; }
}
