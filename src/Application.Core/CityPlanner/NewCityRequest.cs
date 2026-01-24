using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.CityPlanner;

public class NewCityRequest
{
    public required CityId CityId { get; init; }
    public required string Name { get; init; }
    public WonderId WonderId { get; init; } = WonderId.Undefined;
    public int WonderLevel { get; init; }
}
