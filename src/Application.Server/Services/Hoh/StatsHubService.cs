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
        int pageSize = FogConstants.DEFAULT_STATS_PAGE_SIZE, string? name = null, CancellationToken ct = default)
    {
        var query = new GetPlayersWithPaginationQuery()
        {
            PageNumber = pageNumber, PageSize = pageSize, WorldId = worldId, PlayerName = name
        };
        return sender.Send(query, ct);
    }

    public Task<AllianceWithRankings?> GetAllianceAsync(int allianceId)
    {
        var query = new GetAllianceQuery()
        {
            AllianceId = allianceId,
        };
        return sender.Send(query);
    }

    public Task<PaginatedList<AllianceDto>> GetAlliancesAsync(string worldId, int pageNumber = 1,
        int pageSize = FogConstants.DEFAULT_STATS_PAGE_SIZE,
        string? name = null, CancellationToken ct = default)
    {
        var query = new GetAlliancesWithPaginationQuery()
        {
            PageNumber = pageNumber, PageSize = pageSize, WorldId = worldId, AllianceName = name,
        };
        return sender.Send(query, ct);
    }

    public Task<PlayerWithRankings?> GetPlayerAsync(int playerId)
    {
        var query = new GetPlayerQuery()
        {
            PlayerId = playerId,
        };
        return sender.Send(query);
    }
}