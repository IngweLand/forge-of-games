using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class PlayerCityStrategyInfoDto
{
    public required CityId CityId { get; init; }
    public required DateTime EndedAt { get; init; }
    public required DateTime StartedAt { get; init; }
    public required int StrategyId { get; init; }
    public WonderId? Wonder { get; init; }
    public string? WonderName { get; init; }
}
