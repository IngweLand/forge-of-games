using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class AllianceProfileDto
{
    public IReadOnlyCollection<AllianceMemberDto> CurrentMembers { get; init; } = Array.Empty<AllianceMemberDto>();
    public IReadOnlyCollection<StatsTimedStringValue> Names { get; init; } = Array.Empty<StatsTimedStringValue>();
    public required AllianceDto Alliance { get; init; }
}