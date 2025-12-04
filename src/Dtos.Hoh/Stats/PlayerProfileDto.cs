using Ingweland.Fog.Dtos.Hoh.Battle;

namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class PlayerProfileDto
{
    public IReadOnlyCollection<StatsTimedStringValue> Ages { get; init; } = [];
    public IReadOnlyCollection<AllianceDto> Alliances { get; init; } = [];
    public IReadOnlyCollection<DateOnly> CitySnapshotDays { get; init; } = [];
    public bool HasPvpBattles { get; init; }
    public IReadOnlyCollection<string> Names { get; init; } = [];
    public required PlayerDto Player { get; init; }
    public IReadOnlyCollection<ProfileSquadDto> Squads { get; set; } = [];
    public int? TreasureHuntDifficulty { get; set; }
}
