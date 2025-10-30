namespace Ingweland.Fog.Models.Fog.Entities;

public class AllianceAthRanking
{
    public int AllianceId { get; set; }
    public required DateTime CollectedAt { get; set; }
    public int Id { get; set; }
    public required int InGameEventId { get; set; }
    public required int League { get; set; }
    public required int Points { get; set; }
}
