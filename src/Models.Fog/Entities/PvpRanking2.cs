using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class PvpRanking2
{
    public required DateOnly CollectedAt { get; set; }
    public int Id { get; set; }
    public int PlayerId { get; set; }

    public required PvpTier Tier { get; set; }
}
