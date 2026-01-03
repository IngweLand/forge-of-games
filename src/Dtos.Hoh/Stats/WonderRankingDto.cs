using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class WonderRankingDto
{
    public required DateTime EndedAt { get; init; }
    public required int Level { get; init; }
    public required DateTime StartedAt { get; init; }
    public required WonderId Wonder { get; init; }
    public required string WonderName { get; init; }
}
