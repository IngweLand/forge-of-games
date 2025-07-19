namespace Ingweland.Fog.Application.Client.Web.Models;

public record AbilityCostRequest
{
    public required int CurrentLevel { get; init; }
    public required string HeroId { get; init; }
    public required int TargetLevel { get; init; }
}
