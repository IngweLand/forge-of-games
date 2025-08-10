using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Hoh.Entities.Alliance;

public class AllianceMember
{
    public required DateTime JoinedAt { get; init; }
    public required DateTime LastOnline { get; init; }
    public required HohPlayer Player { get; init; }
    public required int RankingPoints { get; init; }
    public required AllianceMemberRole Role { get; init; }
}
