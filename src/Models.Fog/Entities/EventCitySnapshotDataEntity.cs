namespace Ingweland.Fog.Models.Fog.Entities;

public class EventCitySnapshotDataEntity
{
    public required byte[] Data { get; set; }
    public int Id { get; set; }
    public EventCitySnapshot EventCitySnapshot { get; set; }
    public int EventCitySnapshotId { get; set; }
}
