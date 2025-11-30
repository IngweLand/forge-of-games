using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries.Tops;

public record GetTopPlayersQuery : IRequest<IReadOnlyCollection<PlayerDto>>, ICacheableRequest
{
    public required string WorldId { get; init; }
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class GetTopPlayersQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<GetTopPlayersQuery, IReadOnlyCollection<PlayerDto>>
{
    public async Task<IReadOnlyCollection<PlayerDto>> Handle(GetTopPlayersQuery request,
        CancellationToken cancellationToken)
    {
        return await context.Players
            .AsNoTracking()
            .Where(p => p.Status == InGameEntityStatus.Active && p.WorldId == request.WorldId)
            .OrderByDescending(p => p.RankingPoints)
            .ThenBy(p => p.Rank)
            .Skip(0)  // better execution plan
            .Take(FogConstants.DEFAULT_STATS_PAGE_SIZE)
            .ProjectTo<PlayerDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
