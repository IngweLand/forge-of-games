using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class PvpRankingDto
{
    public required DateOnly CollectedAt { get; set; }
    public required PvpTier Tier { get; set; }
}
