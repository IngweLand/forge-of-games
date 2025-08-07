using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Dtos.Hoh.Battle;

public record UserBattleSearchRequest
{
    public required BattleType BattleType { get; init; }
    public int Count { get; init; }
    public BattleSearchRequest? SearchRequest { get; init; }
    public int StartIndex { get; init; }
    public required Guid SubmissionId { get; init; }
}
