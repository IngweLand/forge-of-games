using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class EventCitySnapshot
{
    public required CityId CityId { get; set; }

    public required DateTime CollectedAt { get; set; }

    public required EventCitySnapshotDataEntity Data { get; set; }

    public bool HasPremiumBuildings { get; set; }
    public int Id { get; set; }
    public Player Player { get; set; } = null!;

    public int PlayerId { get; set; }
    public int PremiumExpansionCount { get; set; }
    public required WonderId WonderId { get; set; }
}
