using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class HohCityBasicData
{
    public required string AgeId { get; set; }
    public required string Id { get; init; }
    public required CityId InGameCityId { get; init; }
    public required string Name { get; init; }

    public required DateTime UpdatedAt { get; init; }
}
