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

public record GetAllianceRankingsQuery : IRequest<IReadOnlyCollection<StatsTimedIntValue>>, ICacheableRequest
{
    public required int AllianceId { get; init; }
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class GetAllianceRankingsQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<GetAllianceRankingsQuery, IReadOnlyCollection<StatsTimedIntValue>>
{
    public async Task<IReadOnlyCollection<StatsTimedIntValue>> Handle(GetAllianceRankingsQuery request,
        CancellationToken cancellationToken)
    {
        var statsPeriodStartDate = DateTime.UtcNow.AddDays(FogConstants.DisplayedStatsDays * -1).ToDateOnly();
        return await context.AllianceRankings.Where(x =>
                x.AllianceId == request.AllianceId && x.Type == AllianceRankingType.MemberTotal &&
                x.CollectedAt > statsPeriodStartDate)
            .OrderBy(x => x.CollectedAt)
            .ProjectTo<StatsTimedIntValue>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
