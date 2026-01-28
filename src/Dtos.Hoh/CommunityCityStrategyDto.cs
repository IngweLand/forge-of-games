using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh;

public class CommunityCityStrategyDto
{
    public string? AgeId { get; set; }
    public required string Author { get; set; }
    public required CityId CityId { get; set; }
    public int? GuideId { get; set; }
    public required string Name { get; set; }
    public required string SharedDataId { get; set; }
    public required DateTime UpdatedAt { get; set; }
    public WonderId? WonderId { get; set; }
}
