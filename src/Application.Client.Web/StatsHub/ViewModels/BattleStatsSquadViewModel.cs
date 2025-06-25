namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class BattleStatsSquadViewModel
{
    public BattleStatsValueViewModel? AttackValue { get; set; }
    public BattleStatsValueViewModel? DefenseValue { get; set; }
    public BattleStatsValueViewModel? HealValue { get; set; }

    public required string? HeroName { get; init; }
    public required string? HeroPortraitUrl { get; init; }
    public required string? SupportUnitName { get; init; }
    public required string? SupportUnitPortraitUrl { get; init; }
}
