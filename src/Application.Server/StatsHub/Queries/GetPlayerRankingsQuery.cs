using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetPlayerRankingsQuery : IRequest<IReadOnlyCollection<StatsTimedIntValue>>, ICacheableRequest
{
    public required int PlayerId { get; init; }
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class GetPlayerRankingsQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<GetPlayerRankingsQuery, IReadOnlyCollection<StatsTimedIntValue>>
{
    public async Task<IReadOnlyCollection<StatsTimedIntValue>> Handle(GetPlayerRankingsQuery request,
        CancellationToken cancellationToken)
    {
        var statsPeriodStartDate = DateTime.UtcNow.AddDays(FogConstants.DisplayedStatsDays * -1).ToDateOnly();
        return await context.PlayerRankings.Where(x =>
                x.PlayerId == request.PlayerId && x.Type == PlayerRankingType.TotalHeroPower &&
                x.CollectedAt > statsPeriodStartDate)
            .OrderBy(x => x.CollectedAt)
            .ProjectTo<StatsTimedIntValue>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
