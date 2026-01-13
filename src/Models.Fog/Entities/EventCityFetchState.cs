namespace Ingweland.Fog.Models.Fog.Entities;

public class EventCityFetchState
{
    public required int EventId { get; set; }
    public int FailuresCount { get; set; }
    public ICollection<DateTime> FetchTimestamps { get; set; } = [];
    public required string GameWorldId { get; set; }
    public int Id { get; set; }
    public required int InGamePlayerId { get; set; }
    public required int PlayerId { get; set; }
}
