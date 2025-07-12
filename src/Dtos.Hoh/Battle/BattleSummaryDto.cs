using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

public class BattleSummaryDto
{
    public required string BattleDefinitionId { get; init; }

    // Not for all battle locations
    public Difficulty Difficulty { get; init; }
    public IReadOnlyCollection<BattleSquadDto> EnemySquads { get; init; } = [];
    public required int Id { get; init; }
    public required IReadOnlyCollection<BattleSquadDto> PlayerSquads { get; init; } = [];
    public required BattleResultStatus ResultStatus { get; init; }
    public int? StatsId { get; set; }
}
