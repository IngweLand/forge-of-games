using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class AllianceWithRankings
{
    public IReadOnlyCollection<PlayerDto> CurrentMembers { get; init; } = Array.Empty<PlayerDto>();
    public IReadOnlyCollection<StatsTimedStringValue> Names { get; init; } = Array.Empty<StatsTimedStringValue>();
    public required AllianceDto Alliance { get; init; }
    public IReadOnlyCollection<StatsTimedIntValue> RankingPoints { get; init; } = Array.Empty<StatsTimedIntValue>();
    public DateOnly? RegisteredAt { get; set; }
    public PlayerDto? Leader { get; set; }
}