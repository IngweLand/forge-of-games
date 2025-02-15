namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class PlayerWithRankings
{
    public IReadOnlyCollection<StatsTimedStringValue> Ages { get; init; } = Array.Empty<StatsTimedStringValue>();
    public IReadOnlyCollection<StatsTimedStringValue> Alliances { get; init; } = Array.Empty<StatsTimedStringValue>();
    public IReadOnlyCollection<StatsTimedStringValue> Names { get; init; } = Array.Empty<StatsTimedStringValue>();
    public required PlayerDto Player { get; init; }
    public IReadOnlyCollection<StatsTimedIntValue> RankingPoints { get; init; } = Array.Empty<StatsTimedIntValue>();
    public IReadOnlyCollection<StatsTimedIntValue> ResearchPoints { get; init; } = Array.Empty<StatsTimedIntValue>();
}
