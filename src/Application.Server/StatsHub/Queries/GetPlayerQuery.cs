using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Stats;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetPlayerQuery(int PlayerId) : IRequest<PlayerDto?>, ICacheableRequest
{
    public string CacheKey => $"Player_{PlayerId}";
    public TimeSpan? Duration => TimeSpan.FromHours(6);
    public DateTimeOffset? Expiration { get; }
}

public class GetPlayerQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<GetPlayerQuery, PlayerDto?>
{
    public Task<PlayerDto?> Handle(GetPlayerQuery request, CancellationToken cancellationToken)
    {
        return context.Players.ProjectTo<PlayerDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == request.PlayerId, cancellationToken);
    }
}
