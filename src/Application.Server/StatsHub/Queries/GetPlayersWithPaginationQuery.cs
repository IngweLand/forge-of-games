using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Server.Extensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog;
using MediatR;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetPlayersWithPaginationQuery : IRequest<PaginatedList<PlayerDto>>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public string? PlayerName { get; init; }
    public string? WorldId { get; init; }
}

public class GetPlayersWithPaginationQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<GetPlayersWithPaginationQuery, PaginatedList<PlayerDto>>
{
    public Task<PaginatedList<PlayerDto>> Handle(GetPlayersWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        // TODO implement validator instead
        var pageSize = request.PageSize > 20 ? 20 : request.PageSize;
        var result = context.Players.AsQueryable();
        if (request.WorldId != null)
        {
            result = result.Where(p => p.WorldId == request.WorldId);
        }
        
        if (!string.IsNullOrWhiteSpace(request.PlayerName))
        {
            result = result.Where(p => p.Name.Contains(request.PlayerName));
        }

        return result
            .OrderByDescending(p => p.RankingPoints)
            .ThenBy(p => p.Rank)
            .ProjectTo<PlayerDto>(mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.PageNumber, pageSize, cancellationToken);
    }
}
