using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Server.Extensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog;
using MediatR;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetAlliancesWithPaginationQuery : IRequest<PaginatedList<AllianceDto>>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public string? AllianceName { get; init; }
    public string? WorldId { get; init; }
}

public class GetAlliancesWithPaginationQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<GetAlliancesWithPaginationQuery, PaginatedList<AllianceDto>>
{
    public Task<PaginatedList<AllianceDto>> Handle(GetAlliancesWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        // TODO implement validator instead
        var pageSize = request.PageSize > 20 ? 20 : request.PageSize;
        var result = context.Alliances.AsQueryable();
        if (request.WorldId != null)
        {
            result = result.Where(p => p.WorldId == request.WorldId);
        }
        
        if (!string.IsNullOrWhiteSpace(request.AllianceName))
        {
            result = result.Where(p => p.Name.Contains(request.AllianceName));
        }

        return result
            .OrderByDescending(p => p.RankingPoints)
            .ThenBy(p => p.Rank)
            .ProjectTo<AllianceDto>(mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.PageNumber, pageSize, cancellationToken);
    }
}
