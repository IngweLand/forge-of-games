using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Infrastructure.Entities;

public class AllianceRankingTableEntity : TableEntityBase
{
    public int AvatarIconId { get; init; }
    public int AvatarBackgroundId { get; init; }
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int Points { get; init; }
    public required int Rank { get; init; }
    public int MemberCount { get; init; }
    public required AllianceRankingType Type { get; init; }
}