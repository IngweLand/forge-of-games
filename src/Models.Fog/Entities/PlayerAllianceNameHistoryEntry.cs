namespace Ingweland.Fog.Models.Fog.Entities;

public class PlayerAllianceNameHistoryEntry
{
    public required string AllianceName { get; set; }
    public int Id { get; set; }
    public int PlayerId { get; set; }
}