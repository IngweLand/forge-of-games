using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.PlayerCity.Queries;
using Ingweland.Fog.Application.Server.StatsHub.Queries;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Fog.Entities;
using MediatR;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class StatsHubService(ISender sender) : IStatsHubService
{
    public Task<PaginatedList<PlayerDto>> GetPlayersAsync(string worldId, int startIndex,
        int pageSize = FogConstants.DEFAULT_STATS_PAGE_SIZE, string? name = null, CancellationToken ct = default)
    {
        var query = new GetPlayersWithPaginationQuery
        {
            StartIndex = startIndex, PageSize = pageSize, WorldId = worldId, Name = name,
        };
        return sender.Send(query, ct);
    }

    public Task<AllianceWithRankings?> GetAllianceAsync(int allianceId, CancellationToken ct = default)
    {
        var query = new GetAllianceQuery
        {
            AllianceId = allianceId,
        };
        return sender.Send(query, ct);
    }

    public Task<PaginatedList<AllianceDto>> GetAlliancesAsync(string worldId, int startIndex,
        int pageSize = FogConstants.DEFAULT_STATS_PAGE_SIZE,
        string? name = null, CancellationToken ct = default)
    {
        var query = new GetAlliancesWithPaginationQuery
        {
            StartIndex = startIndex, PageSize = pageSize, WorldId = worldId, Name = name,
        };
        return sender.Send(query, ct);
    }

    public Task<LeaderboardTopItemsDto> GetAllLeaderboardTopItemsAsync(CancellationToken ct = default)
    {
        return sender.Send(new GetAllLeaderboardTopItemsQuery(), ct);
    }

    public Task<PaginatedList<PvpBattleDto>> GetPlayerBattlesAsync(int playerId, int startIndex = 0,
        int count = FogConstants.DEFAULT_STATS_PAGE_SIZE, CancellationToken ct = default)
    {
        var query = new GetPlayerBattlesQuery
        {
            PlayerId = playerId,
            StartIndex = startIndex,
            Count = count,
        };
        return sender.Send(query, ct);
    }

    public Task<PlayerDto?> GetPlayerAsync(int playerId, CancellationToken ct = default)
    {
        var query = new GetPlayerQuery(playerId);
        return sender.Send(query, ct);
    }

    public Task<PlayerProfile?> GetPlayerProfileAsync(int playerId, CancellationToken ct = default)
    {
        var query = new GetPlayerProfileQuery
        {
            PlayerId = playerId,
        };
        return sender.Send(query, ct);
    }

    public Task<HohCity?> GetPlayerCityAsync(int playerId, CancellationToken ct = default)
    {
        var query = new GetPlayerCityQuery(playerId);
        return sender.Send(query, ct);
    }

    public Task<IReadOnlyCollection<string>> GetTopHeroesAsync(string? ageId = null, int? fromLevel = null,
        int? toLevel  = null, CancellationToken ct = default)
    {
        return sender.Send(new GetTopHeroesQuery
        {
            AgeId = ageId,
            FromLevel = fromLevel,
            ToLevel = toLevel,
        }, ct);
    }
}
