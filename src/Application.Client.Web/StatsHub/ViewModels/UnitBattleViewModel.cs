using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class UnitBattleViewModel
{
    public int AbilityLevel { get; init; }
    public string AttackValue { get; init; } = string.Empty;
    public required string BattleDefinitionId { get; init; }
    public required BattleType BattleType { get; init; }
    public required string BattleTypeName { get; init; }
    public string DefenseValue { get; init; } = string.Empty;
    public Difficulty Difficulty { get; init; }
    public string HealValue { get; init; } = string.Empty;
    public required HeroLevelSpecs Level { get; init; }
    public required string UnitId { get; init; }
}
