namespace Ingweland.Fog.Models.Fog.Entities;

public class PvpRanking
{
    public required DateTime CollectedAt { get; set; }
    public int Id { get; set; }
    public int PlayerId { get; set; }

    public required int Points { get; set; }
    public required int Rank { get; set; }
}