using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetEventCityRankingsQuery : IRequest<PaginatedList<PlayerDto>>, ICacheableRequest
{
    public required string WorldId { get; init; }
    public TimeSpan? Duration => TimeSpan.FromHours(1);
    public DateTimeOffset? Expiration { get; }
}

public class GetEventCityRankingsQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<GetEventCityRankingsQuery, PaginatedList<PlayerDto>>
{
    public async Task<PaginatedList<PlayerDto>> Handle(GetEventCityRankingsQuery request,
        CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.ToDateOnly();
        var rankings = await context.PlayerRankings
            .Include(x => x.Player)
            .ThenInclude(x => x.AllianceMembership)
            .Where(x => x.Type == PlayerRankingType.EventCityProgress && x.CollectedAt == today &&
                x.Player.WorldId == request.WorldId)
            .OrderByDescending(x => x.Points)
            .Take(FogConstants.MAX_EVENT_CITY_RANKINGS)
            .ProjectTo<PlayerDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PaginatedList<PlayerDto>(rankings, 0, rankings.Count);
    }
}
