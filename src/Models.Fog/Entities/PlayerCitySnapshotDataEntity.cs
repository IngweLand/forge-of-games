namespace Ingweland.Fog.Models.Fog.Entities;

public class PlayerCitySnapshotDataEntity
{
    public required byte[] Data { get; set; }
    public int Id { get; set; }
    public PlayerCitySnapshot PlayerCitySnapshot { get; set; }
    public int PlayerCitySnapshotId { get; set; }
}
