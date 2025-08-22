namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class BattleTimelineGroupViewModel
{
    public required string Time { get; init; }
    public required IReadOnlyCollection<BattleTimelineEntryViewModel> Entries { get; init; }
}
