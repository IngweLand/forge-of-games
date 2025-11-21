using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetPlayerPvpRankingsQuery : IRequest<IReadOnlyCollection<PvpRankingDto>>, ICacheableRequest
{
    public required int PlayerId { get; init; }
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class GetPlayerPvpRankingQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<GetPlayerPvpRankingsQuery, IReadOnlyCollection<PvpRankingDto>>
{
    public async Task<IReadOnlyCollection<PvpRankingDto>> Handle(GetPlayerPvpRankingsQuery request,
        CancellationToken cancellationToken)
    {
        var cutOffDate = DateTime.UtcNow.AddDays(FogConstants.DisplayedStatsDays * -1).ToDateOnly();
        return await context.PvpRankings
            .Where(x => x.PlayerId == request.PlayerId && x.CollectedAt >= cutOffDate)
            .OrderBy(x => x.CollectedAt)
            .ProjectTo<PvpRankingDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
