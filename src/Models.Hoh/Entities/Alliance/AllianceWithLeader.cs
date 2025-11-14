namespace Ingweland.Fog.Models.Hoh.Entities.Alliance;

public class AllianceWithLeader
{
    public required HohAllianceExtended Alliance { get; init; }
    public required AllianceMember Leader { get; init; }
}
