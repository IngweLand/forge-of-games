using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Fog.Entities;

public class AllianceMemberEntity
{
    public Alliance Alliance { get; set; } = null!;
    public int AllianceId { get; set; }
    public int Id { get; set; }
    public DateTime? JoinedAt { get; set; }
    public Player Player { get; set; } = null!;
    public int PlayerId { get; set; }
    public AllianceMemberRole Role { get; set; } = AllianceMemberRole.AllianceTrainee;
}
