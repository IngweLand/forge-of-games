using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class BattleSummaryViewModel
{
    public required int Id { get; init; }

    public required IReadOnlyCollection<BattleHeroViewModel> PlayerSquads { get; set; } =
        new List<BattleHeroViewModel>();

    public required BattleResultStatus ResultStatus { get; set; }
    public int? StatsId { get; init; }
}
