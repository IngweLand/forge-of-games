namespace Ingweland.Fog.Models.Fog.Entities;

public class AllianceNameHistoryEntry : IHistoryEntry
{
    public int AllianceId { get; set; }
    public int Id { get; set; }
    public required string Name { get; set; }
    public required DateTime ChangedAt { get; set; }
}