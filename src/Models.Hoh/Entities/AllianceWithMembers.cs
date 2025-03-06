namespace Ingweland.Fog.Models.Hoh.Entities;

public class AllianceWithMembers
{
    public required int AllianceId { get; init; }
    public required IReadOnlyCollection<AllianceMember> Members { get; init; }
}