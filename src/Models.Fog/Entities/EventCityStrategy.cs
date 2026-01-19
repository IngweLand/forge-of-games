using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class EventCityStrategy
{
    public required CityId CityId { get; set; }

    public required EventCityStrategyDataEntity Data { get; set; }

    public bool HasPremiumBuildings { get; set; }
    public bool HasPremiumExpansion { get; set; }
    public int Id { get; set; }

    public InGameEventEntity InGameEvent { get; set; } = null!;
    public int InGameEventId { get; set; }
    public Player Player { get; set; } = null!;

    public int PlayerId { get; set; }
    public required WonderId WonderId { get; set; }
}
