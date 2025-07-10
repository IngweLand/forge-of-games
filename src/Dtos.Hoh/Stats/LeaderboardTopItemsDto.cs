using Ingweland.Fog.Models.Fog;

namespace Ingweland.Fog.Dtos.Hoh.Stats;

public class LeaderboardTopItemsDto
{
    public PaginatedList<AllianceDto> BetaWorldAlliances { get; init; } = PaginatedList<AllianceDto>.Empty;
    public PaginatedList<PlayerDto> BetaWorldPlayers { get; init; } = PaginatedList<PlayerDto>.Empty;
    public PaginatedList<AllianceDto> MainWorldAlliances { get; init; } = PaginatedList<AllianceDto>.Empty;
    public PaginatedList<PlayerDto> MainWorldPlayers { get; init; } = PaginatedList<PlayerDto>.Empty;
}
