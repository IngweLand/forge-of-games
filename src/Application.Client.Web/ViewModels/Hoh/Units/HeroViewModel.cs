using Ingweland.Fog.Dtos.Hoh.Units;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;

public class HeroViewModel
{
    public required string Id { get; init; }
    public required IReadOnlyCollection<HeroLevelViewModel> Levels { get; init; }
    public required string Name { get; init; }
    public string? PortraitUrl { get; init; }

    public int StarCount { get; init; }
    public required string UnitTypeIconUrl { get; init; }
    public required string UnitTypeName { get; init; }
    public string? VideoUrl { get; init; }
    public string? ImageUrl { get; init; }
    public required HeroDto Data { get; init; }
}
