namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class AllianceAthRankingDto
{
    public required int League { get; init; }
    public required int Points { get; init; }
    public required DateTime StartedAt { get; init; }
    public required DateTime EndedAt { get; init; }
}
