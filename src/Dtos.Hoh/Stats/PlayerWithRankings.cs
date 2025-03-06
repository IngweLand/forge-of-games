namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class PlayerWithRankings
{
    public IReadOnlyCollection<StatsTimedStringValue> Ages { get; init; } = Array.Empty<StatsTimedStringValue>();
    public IReadOnlyCollection<string> Alliances { get; init; } = Array.Empty<string>();
    public IReadOnlyCollection<string> Names { get; init; } = Array.Empty<string>();
    public required PlayerDto Player { get; init; }
    public IReadOnlyCollection<StatsTimedIntValue> RankingPoints { get; init; } = Array.Empty<StatsTimedIntValue>();
    public IReadOnlyCollection<StatsTimedIntValue> PvpRankingPoints { get; init; } = Array.Empty<StatsTimedIntValue>();
}
