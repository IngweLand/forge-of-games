namespace Ingweland.Fog.Models.Fog.Entities;

public class BattleTimelineEntity
{
    public int Id { get; set; }
    public required byte[] InGameBattleId { get; set; }
    public required ISet<BattleTimelineEntry> Entries { get; set; }
}
