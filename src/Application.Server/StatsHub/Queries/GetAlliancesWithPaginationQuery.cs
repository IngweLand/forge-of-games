using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Extensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetAlliancesWithPaginationQuery : IRequest<PaginatedList<AllianceDto>>
{
    public string? Name { get; init; }
    public int PageSize { get; init; }
    public int StartIndex { get; init; }
    public string? WorldId { get; init; }
}

public class GetAlliancesWithPaginationQueryHandler(
    IFogDbContext context,
    IMapper mapper,
    IInGameAllianceService inGameAllianceService,
    IFogAllianceService fogAllianceService,
    ILogger<GetAlliancesWithPaginationQueryHandler> logger)
    : IRequestHandler<GetAlliancesWithPaginationQuery, PaginatedList<AllianceDto>>
{
    public async Task<PaginatedList<AllianceDto>> Handle(GetAlliancesWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        // TODO implement validator instead
        var pageSize = request.PageSize > 100 ? 100 : request.PageSize;
        var result = await GetAlliancesFromDbAsync(request.WorldId, request.Name, request.StartIndex, pageSize,
            cancellationToken);
        if (result.Count != 0 || request.WorldId == null || string.IsNullOrWhiteSpace(request.Name) ||
            request.Name.Length < FogConstants.ALLIANCE_NAME_MIN_IN_GAME_SEARCH_STRING_LENGTH)
        {
            return result;
        }

        logger.LogInformation("Could not find alliances with {searchString} in name in world {worldId}",
            request.Name, request.WorldId);

        var allianceSearchResult =
            await inGameAllianceService.SearchAlliancesAsync(request.WorldId, request.Name);

        allianceSearchResult.LogIfFailed<GetAlliancesWithPaginationQueryHandler>();

        if (allianceSearchResult.IsFailed || allianceSearchResult.Value.Count == 0)
        {
            return PaginatedList<AllianceDto>.Empty;
        }

        var now = DateTime.UtcNow;
        foreach (var searchResult in allianceSearchResult.Value)
        {
            await fogAllianceService.UpsertAlliance(searchResult.Alliance, request.WorldId, now);
        }

        result = await GetAlliancesFromDbAsync(request.WorldId, request.Name, request.StartIndex, pageSize,
            cancellationToken);

        return result;
    }

    private Task<PaginatedList<AllianceDto>> GetAlliancesFromDbAsync(string? worldId, string? searchString,
        int startIndex,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var result = context.Alliances.AsQueryable();
        if (worldId != null)
        {
            result = result.Where(p => p.WorldId == worldId);
        }

        var allianceName = searchString?.Trim();
        if (!string.IsNullOrWhiteSpace(allianceName))
        {
            result = result.Where(p => p.Name.Contains(allianceName));
        }

        return result
            .OrderBy(x => x.Status)
            .ThenByDescending(p => p.RankingPoints)
            .ProjectTo<AllianceDto>(mapper.ConfigurationProvider)
            .ToPaginatedListAsync(startIndex, pageSize, cancellationToken);
    }
}
