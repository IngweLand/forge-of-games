using Ingweland.Fog.Models.Hoh.Entities.Abstractions;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public record HeroProfileBasicViewModel
{
    public required int AbilityLevel { get; init; }
    public required IBattleUnitProperties Hero { get; init; }
    public required string HeroUnitId { get; init; }
    public required string Level { get; init; }
    public required string Name { get; init; }
    public required string PortraitUrl { get; init; }
    public int StarCount { get; init; }
    public IBattleUnitProperties? SupportUnit { get; init; }
    public required string UnitClassIconUrl { get; init; }

    public required string UnitColor { get; init; }
    public required string UnitTypeIconUrl { get; init; }
}
