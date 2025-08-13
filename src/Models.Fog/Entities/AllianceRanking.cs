using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class AllianceRanking
{
    public int AllianceId { get; set; }
    public required DateOnly CollectedAt { get; set; }
    public int Id { get; set; }
    public required int Points { get; set; }
    public required int Rank { get; set; }
    public required AllianceRankingType Type { get; set; }
}