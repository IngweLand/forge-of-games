namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class AllianceWithRankings
{
    public IReadOnlyCollection<PlayerDto> CurrentMembers { get; init; } = Array.Empty<PlayerDto>();
    public IReadOnlyCollection<PlayerDto> PastMembers { get; init; } = Array.Empty<PlayerDto>();
    public IReadOnlyCollection<StatsTimedStringValue> Names { get; init; } = Array.Empty<StatsTimedStringValue>();
    public required AllianceDto Alliance { get; init; }
    public IReadOnlyCollection<StatsTimedIntValue> RankingPoints { get; init; } = Array.Empty<StatsTimedIntValue>();
}