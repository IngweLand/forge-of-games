using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Hoh.Entities.Alliance;

public class HohAllianceExtended : HohAlliance
{
    public string? Description { get; init; }
    public string? Language { get; init; }
    public required int Rank { get; init; }
    public required AllianceAdmissionType AdmissionType { get; init; }
}
