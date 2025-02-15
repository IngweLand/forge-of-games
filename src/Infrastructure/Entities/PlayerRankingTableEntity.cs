using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Infrastructure.Entities;

public class PlayerRankingTableEntity : TableEntityBase
{
    public required string Age { get; init; }
    public string? AllianceName { get; init; }
    public required int AvatarId { get; init; }
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int Points { get; init; }
    public required int Rank { get; init; }
    public required PlayerRankingType Type { get; init; }
}
