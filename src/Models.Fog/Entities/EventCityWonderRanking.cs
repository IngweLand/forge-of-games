namespace Ingweland.Fog.Models.Fog.Entities;

public class EventCityWonderRanking
{
    public required DateTime CollectedAt { get; set; }
    public int Id { get; set; }
    public required int InGameEventId { get; set; }
    public int PlayerId { get; set; }
    public required int WonderLevel { get; set; }
}
