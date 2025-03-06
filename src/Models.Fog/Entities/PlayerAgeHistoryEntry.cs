namespace Ingweland.Fog.Models.Fog.Entities;

public class PlayerAgeHistoryEntry : IHistoryEntry
{
    public required DateTime ChangedAt { get; set; }
    public int Id { get; set; }
    public required string Age { get; set; }
    public int PlayerId { get; set; }
}