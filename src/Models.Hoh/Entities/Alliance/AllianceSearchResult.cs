namespace Ingweland.Fog.Models.Hoh.Entities.Alliance;

public class AllianceSearchResult
{
    public required IReadOnlyCollection<AllianceMember> Members { get; init; }
    public required HohAllianceExtended Alliance { get; init; }
}
