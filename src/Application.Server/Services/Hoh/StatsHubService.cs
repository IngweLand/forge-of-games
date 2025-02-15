using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.StatsHub.Queries;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Fog.Entities;
using MediatR;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class StatsHubService(ISender sender) : IStatsHubService
{
    public Task<PaginatedList<PlayerDto>> GetPlayersAsync(string worldId, int pageNumber = 1,
        int pageSize = FogConstants.DEFAULT_STATS_PAGE_SIZE, string? playerName = null, CancellationToken ct = default)
    {
        var query = new GetPlayersWithPaginationQuery()
        {
            PageNumber = pageNumber, PageSize = pageSize, WorldId = worldId, PlayerName = playerName,
        };
        return sender.Send(query, ct);
    }

    public Task<PlayerWithRankings?> GetPlayerAsync(string worldId, int inGamePlayerId)
    {
        var query = new GetPlayerQuery()
        {
            PlayerKey = new PlayerKey(worldId, inGamePlayerId),
        };
        return sender.Send(query);
    }
}
