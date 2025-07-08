using Ingweland.Fog.Models.Fog;

namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class LeaderboardTopItemsDto
{
    public static readonly LeaderboardTopItemsDto Blank = new();
    public PaginatedList<AllianceDto> BetaWorldAlliances { get; init; } = new([], 0, 0, 0);
    public PaginatedList<PlayerDto> BetaWorldPlayers { get; init; } = new([], 0, 0, 0);
    public PaginatedList<AllianceDto> MainWorldAlliances { get; init; } = new([], 0, 0, 0);
    public PaginatedList<PlayerDto> MainWorldPlayers { get; init; } = new([], 0, 0, 0);
}
