using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh;

public class CommunityCityGuideInfoDto
{
    public string? AgeId { get; init; }
    public required string Author { get; init; }
    public required CityId CityId { get; init; }
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required DateTime UpdatedAt { get; init; }
    public WonderId? WonderId { get; init; }
}
