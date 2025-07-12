using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class BattleHeroViewModel
{
    public int AbilityLevel { get; init; }
    public required string Id { get; set; }
    public required HeroLevelSpecs Level { get; init; }

    public required string Name { get; set; }
    public required string PortraitUrl { get; init; }
    public int StarCount { get; init; }
    public required string UnitClassIconUrl { get; set; }

    public required string UnitColor { get; set; }

    public required string UnitTypeIconUrl { get; init; }
}
