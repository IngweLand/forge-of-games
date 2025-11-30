using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries.Tops;

public record GetTopAlliancesQuery : IRequest<IReadOnlyCollection<AllianceDto>>, ICacheableRequest
{
    public required string WorldId { get; init; }
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class GetTopAlliancesQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<GetTopAlliancesQuery, IReadOnlyCollection<AllianceDto>>
{
    public async Task<IReadOnlyCollection<AllianceDto>> Handle(GetTopAlliancesQuery request,
        CancellationToken cancellationToken)
    {
        return await context.Alliances
            .AsNoTracking()
            .Where(p => p.Status == InGameEntityStatus.Active && p.WorldId == request.WorldId)
            .OrderByDescending(p => p.RankingPoints)
            .Skip(0) // better execution plan
            .Take(FogConstants.DEFAULT_STATS_PAGE_SIZE)
            .ProjectTo<AllianceDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
