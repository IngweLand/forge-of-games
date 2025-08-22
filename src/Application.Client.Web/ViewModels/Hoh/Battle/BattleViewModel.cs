namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class BattleViewModel
{
    public required BattleSummaryViewModel Summary { get; init; }
    public IReadOnlyCollection<BattleTimelineGroupViewModel> Timeline { get; set; } = [];
}
