namespace Ingweland.Fog.Models.Hoh.Entities.Alliance;

public class HohAlliance
{
    public required AllianceBanner Banner { get; init; }
    public required int Id { get; init; }
    public int MemberCount { get; set; }
    public required string Name { get; init; }
}
