using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Server.Extensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Fog.Enums;
using MediatR;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetAlliancesWithPaginationQuery : IRequest<PaginatedList<AllianceDto>>
{
    public int StartIndex { get; init; }
    public int PageSize { get; init; }
    public string? Name { get; init; }
    public string? WorldId { get; init; }
}

public class GetAlliancesWithPaginationQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<GetAlliancesWithPaginationQuery, PaginatedList<AllianceDto>>
{
    public Task<PaginatedList<AllianceDto>> Handle(GetAlliancesWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        // TODO implement validator instead
        var pageSize = request.PageSize > 100 ? 100 :request.PageSize;
        var result = context.Alliances.AsQueryable();
        if (request.WorldId != null)
        {
            result = result.Where(p => p.WorldId == request.WorldId);
        }
        
        var allianceName = request.Name?.Trim();
        if (!string.IsNullOrWhiteSpace(allianceName))
        {
            result = result.Where(p => p.Name.Contains(allianceName));
        }

        return result
            .OrderBy(x => x.Status)
            .ThenByDescending(p => p.RankingPoints)
            .ProjectTo<AllianceDto>(mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.StartIndex, pageSize, cancellationToken);
    }
}
