using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class BattleSummaryViewModel
{
    public required string BattleDefinitionId { get; init; }
    public required BattleType BattleType { get; init; }
    public Difficulty Difficulty { get; init; }
    public IReadOnlyCollection<BattleSquadViewModel> EnemySquads { get; init; } = [];
    public required int Id { get; init; }
    public required string PerformedAt { get; init; }

    public required IReadOnlyCollection<BattleSquadViewModel> PlayerSquads { get; init; } = [];

    public required BattleResultStatus ResultStatus { get; init; }
    public int? StatsId { get; init; }
}
