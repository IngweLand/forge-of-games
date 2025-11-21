using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class AllianceAthRankingDto
{
    public required TreasureHuntLeague League { get; init; }
    public required int Points { get; init; }
    public required DateTime StartedAt { get; init; }
    public required DateTime EndedAt { get; init; }
}
