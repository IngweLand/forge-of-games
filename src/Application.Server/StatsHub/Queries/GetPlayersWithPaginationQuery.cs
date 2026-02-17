using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Extensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Fog.Enums;
using MediatR;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetPlayersWithPaginationQuery : IRequest<PaginatedList<PlayerDto>>, ICacheableRequest
{
    public string? Name { get; init; }
    public int PageSize { get; init; }
    public int StartIndex { get; init; }
    public string? WorldId { get; init; }
    public TimeSpan? Duration => TimeSpan.FromMinutes(15);
    public DateTimeOffset? Expiration { get; }
}

public class GetPlayersWithPaginationQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<GetPlayersWithPaginationQuery, PaginatedList<PlayerDto>>
{
    public Task<PaginatedList<PlayerDto>> Handle(GetPlayersWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        // TODO implement validator instead
        var pageSize = request.PageSize > FogConstants.MAX_LEADERBOARD_PAGE_SIZE
            ? FogConstants.MAX_LEADERBOARD_PAGE_SIZE
            : request.PageSize;
        var result = context.Players.Where(p => p.Status == InGameEntityStatus.Active);
        if (request.WorldId != null)
        {
            result = result.Where(p => p.WorldId == request.WorldId);
        }

        var playerName = request.Name?.Trim();
        if (!string.IsNullOrWhiteSpace(playerName))
        {
            result = result.Where(p => p.Name.Contains(playerName));
            if (pageSize > FogConstants.LEADERBOARD_SEARCH_RESULT_PAGE_SIZE)
            {
                pageSize = FogConstants.LEADERBOARD_SEARCH_RESULT_PAGE_SIZE;
            }
        }

        return result
            .OrderByDescending(p => p.RankingPoints)
            .ThenBy(p => p.Rank)
            .ProjectTo<PlayerDto>(mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.StartIndex, pageSize, cancellationToken);
    }
}
