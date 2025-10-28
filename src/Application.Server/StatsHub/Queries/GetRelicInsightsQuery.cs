using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Stats;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetRelicInsightsQuery(string UnitId)
    : IRequest<IReadOnlyCollection<RelicInsightsDto>>, ICacheableRequest
{
    public TimeSpan? Duration => TimeSpan.FromHours(24);
    public DateTimeOffset? Expiration { get; }
}

public class GetRelicInsightsQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<GetRelicInsightsQuery, IReadOnlyCollection<RelicInsightsDto>>
{
    public async Task<IReadOnlyCollection<RelicInsightsDto>> Handle(GetRelicInsightsQuery request,
        CancellationToken cancellationToken)
    {
        var items = await context.RelicInsights.Where(x => x.UnitId == request.UnitId)
            .ToListAsync(cancellationToken);
        return mapper.Map<IReadOnlyCollection<RelicInsightsDto>>(items);
    }
}
