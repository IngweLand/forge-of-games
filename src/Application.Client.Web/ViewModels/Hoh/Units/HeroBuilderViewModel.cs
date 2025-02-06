using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;

public class HeroBuilderViewModel
{
    public required BattleAbilityDto Ability { get; init; }
    public required IReadOnlyCollection<int> AbilityLevels { get; init; }
    public required IReadOnlyCollection<int> AwakeningLevels { get; init; }
    public required IReadOnlyCollection<BuildingDto> Barracks { get; init; }
    public required IReadOnlyCollection<int> BarracksLevels { get; init; }
    public required HeroDto Hero { get; init; }
    public required IReadOnlyCollection<HeroLevelSpecs> HeroLevels { get; init; }
}
