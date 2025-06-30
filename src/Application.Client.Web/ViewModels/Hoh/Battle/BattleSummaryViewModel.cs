using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class BattleSummaryViewModel
{
    public IReadOnlyCollection<BattleHeroViewModel> EnemySquads { get; init; } = [];
    public required int Id { get; init; }

    public required IReadOnlyCollection<BattleHeroViewModel> PlayerSquads { get; init; } = [];

    public required BattleResultStatus ResultStatus { get; init; }
    public int? StatsId { get; init; }
}
