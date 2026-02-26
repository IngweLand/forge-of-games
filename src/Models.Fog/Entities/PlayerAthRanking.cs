namespace Ingweland.Fog.Models.Fog.Entities;

public class PlayerAthRanking
{
    public int Id { get; set; }
    public required int InGameEventId { get; set; }
    public int PlayerId { get; set; }
    public required int Points { get; set; }
    public required DateTime UpdatedAt { get; set; }
}
