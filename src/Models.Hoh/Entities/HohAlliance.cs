namespace Ingweland.Fog.Models.Hoh.Entities;

public class HohAlliance
{
    public int AvatarBackgroundId { get; init; }
    public int AvatarIconId { get; init; }
    public required int Id { get; init; }
    public int MemberCount { get; set; }
    public required string Name { get; init; }
}