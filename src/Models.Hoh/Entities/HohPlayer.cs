namespace Ingweland.Fog.Models.Hoh.Entities;

public class HohPlayer
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public int AvatarId { get; init; }
    public required string Age { get; init; }
    public string? Locale { get; init; }
}