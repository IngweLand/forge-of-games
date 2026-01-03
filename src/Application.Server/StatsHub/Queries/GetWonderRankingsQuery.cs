using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.StatsHub.Factories;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Shared.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetWonderRankingsQuery(int PlayerId) : IRequest<IReadOnlyCollection<WonderRankingDto>>, ICacheableRequest
{
    public TimeSpan? Duration { get; }
    public DateTimeOffset? Expiration => DateTimeUtils.GetNextNoonUtc();
}

public class GetWonderRankingsQueryHandler(
    IFogDbContext context,
    IWonderRankingDtoFactory wonderRankingDtoFactory,
    ILogger<GetAllianceQueryHandler> logger)
    : IRequestHandler<GetWonderRankingsQuery, IReadOnlyCollection<WonderRankingDto>>
{
    public async Task<IReadOnlyCollection<WonderRankingDto>> Handle(GetWonderRankingsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Getting player: {PlayerId}", request.PlayerId);
        var existingPlayer = await context.Players
            .Include(x =>
                x.EventCityWonderRankings.OrderByDescending(y => y.InGameEventId).Take(FogConstants.MAX_DISPLAYED_WONDER_RANKINGS))
            .FirstOrDefaultAsync(x => x.Id == request.PlayerId, cancellationToken);
        if (existingPlayer == null)
        {
            logger.LogInformation("Player with ID {PlayerId} not found", request.PlayerId);
            return [];
        }

        if (existingPlayer.EventCityWonderRankings.Count == 0)
        {
            return [];
        }

        var eventIds = existingPlayer.EventCityWonderRankings.Select(x => x.InGameEventId).ToHashSet();
        var events = await context.InGameEvents.Where(x => eventIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);
        
        return existingPlayer.EventCityWonderRankings.Where(x => events.ContainsKey(x.InGameEventId))
            .Select(x => wonderRankingDtoFactory.Create(x, events[x.InGameEventId]))
            .ToList();
    }
}
