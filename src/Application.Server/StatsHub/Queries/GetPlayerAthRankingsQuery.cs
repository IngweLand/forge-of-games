using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.StatsHub.Factories;
using Ingweland.Fog.Dtos.Hoh.Stats;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetPlayerAthRankingsQuery : IRequest<IReadOnlyCollection<PlayerAthRankingDto>>, ICacheableRequest
{
    public required int PlayerId { get; init; }
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class GetPlayerAthRankingsQueryHandler(
    IFogDbContext context,
    IPlayerAthRankingDtoFactory athRankingDtoFactory,
    ILogger<GetAllianceQueryHandler> logger)
    : IRequestHandler<GetPlayerAthRankingsQuery, IReadOnlyCollection<PlayerAthRankingDto>>
{
    public async Task<IReadOnlyCollection<PlayerAthRankingDto>> Handle(GetPlayerAthRankingsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Getting player: {PlayerId}", request.PlayerId);
        var existingPlayer = await context.Players
            .Include(x =>
                x.AthRankings.OrderByDescending(y => y.InGameEventId).Take(FogConstants.MAX_DISPLAYED_ATH_EVENTS))
            .FirstOrDefaultAsync(x => x.Id == request.PlayerId, cancellationToken);
        if (existingPlayer == null)
        {
            logger.LogInformation("Player with ID {PlayerId} not found", request.PlayerId);
            return [];
        }

        if (existingPlayer.AthRankings.Count == 0)
        {
            return [];
        }

        var eventIds = existingPlayer.AthRankings.Select(x => x.InGameEventId).ToHashSet();
        var events = await context.InGameEvents.Where(x => eventIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        return existingPlayer.AthRankings.Where(x => events.ContainsKey(x.InGameEventId))
            .Select(x => athRankingDtoFactory.Create(x, events[x.InGameEventId]))
            .ToList();
    }
}
