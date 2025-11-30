using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class PlayerRanking
{
    public required DateOnly CollectedAt { get; set; }
    public int Id { get; set; }
    public Player Player { get; set; } = null!;
    public int PlayerId { get; set; }
    public required int Points { get; set; }
    public int Rank { get; set; }
    public required PlayerRankingType Type { get; set; }
}
