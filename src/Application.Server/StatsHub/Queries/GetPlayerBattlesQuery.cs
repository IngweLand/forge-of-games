using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.StatsHub.Factories;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetPlayerBattlesQuery : IRequest<PaginatedList<PvpBattleDto>>, ICacheableRequest
{
    public int Count { get; init; }
    public required int PlayerId { get; init; }
    public int StartIndex { get; init; }
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class GetPlayerBattlesQueryHandler(
    IFogDbContext context,
    IPlayerBattlesFactory playerBattlesFactory,
    IBattleQueryService battleQueryService)
    : IRequestHandler<GetPlayerBattlesQuery, PaginatedList<PvpBattleDto>>
{
    public async Task<PaginatedList<PvpBattleDto>> Handle(GetPlayerBattlesQuery request,
        CancellationToken cancellationToken)
    {
        var source = context.PvpBattles.AsNoTracking()
            .Include(b => b.Winner)
            .Include(b => b.Loser)
            .Where(b => b.WinnerId == request.PlayerId || b.LoserId == request.PlayerId);
        var count = await source.CountAsync(cancellationToken);

        if (count == 0)
        {
            return PaginatedList<PvpBattleDto>.Empty;
        }

        var requestCount = Math.Min(request.Count, 15);
        var battles = await source.OrderByDescending(b => b.PerformedAt)
            .Skip(request.StartIndex)
            .Take(requestCount)
            .ToListAsync(cancellationToken);
        var battleIds = battles.Select(src => src.InGameBattleId);
        var existingStatsIds = await battleQueryService.GetExistingBattleStatsIdsAsync(battleIds, cancellationToken);

        var battleDtos = battles.Select(x =>
        {
            int? statsId = null;
            if (existingStatsIds.TryGetValue(x.InGameBattleId, out var value))
            {
                statsId = value;
            }

            return playerBattlesFactory.Create(x, statsId);
        }).ToList();

        return new PaginatedList<PvpBattleDto>(battleDtos, request.StartIndex, count);
    }
}
