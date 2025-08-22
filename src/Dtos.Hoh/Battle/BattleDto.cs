namespace Ingweland.Fog.Dtos.Hoh.Battle;

public class BattleDto
{
    public required BattleSummaryDto Summary { get; init; }
    public IReadOnlyCollection<BattleTimelineEntryDto> Timeline { get; init; } = [];
}
