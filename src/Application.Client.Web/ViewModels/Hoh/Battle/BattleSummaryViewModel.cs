using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class BattleSummaryViewModel
{
    public IReadOnlyCollection<HeroProfileViewModel> EnemySquads { get; init; } = [];
    public required int Id { get; init; }

    public required IReadOnlyCollection<HeroProfileViewModel> PlayerSquads { get; init; } = [];

    public required BattleResultStatus ResultStatus { get; init; }
    public int? StatsId { get; init; }
}
