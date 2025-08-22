using Ingweland.Fog.Models.Fog.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class BattleTimelineEntryViewModel
{
    public required string AbilityIconUrl { get; init; }
    public required BattleSquadSide Side { get; init; }
    public required BattleSquadViewModel Squad { get; init; }
    public required string Title { get; init; }
}
