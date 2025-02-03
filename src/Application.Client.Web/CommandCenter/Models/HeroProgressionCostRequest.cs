using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Models;

public class HeroProgressionCostRequest
{
    public required HeroLevelSpecs CurrentLevel { get; init; }
    public required string HeroId { get; init; }
    public required HeroLevelSpecs TargetLevel { get; init; }
}
