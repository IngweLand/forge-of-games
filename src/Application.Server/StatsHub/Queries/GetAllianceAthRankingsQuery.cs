using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.StatsHub.Factories;
using Ingweland.Fog.Dtos.Hoh.Stats;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetAllianceAthRankingsQuery : IRequest<IReadOnlyCollection<AllianceAthRankingDto>>, ICacheableRequest
{
    public required int AllianceId { get; init; }
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class GetAllianceAthRankingsQueryHandler(
    IFogDbContext context,
    IAllianceAthRankingDtoFactory athRankingDtoFactory,
    ILogger<GetAllianceQueryHandler> logger)
    : IRequestHandler<GetAllianceAthRankingsQuery, IReadOnlyCollection<AllianceAthRankingDto>>
{
    public async Task<IReadOnlyCollection<AllianceAthRankingDto>> Handle(GetAllianceAthRankingsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Getting alliance: {AllianceId}", request.AllianceId);
        var existingAlliance = await context.Alliances
            .Include(x =>
                x.AthRankings.OrderByDescending(y => y.InGameEventId).Take(FogConstants.MAX_DISPLAYED_ATH_EVENTS))
            .FirstOrDefaultAsync(x => x.Id == request.AllianceId, cancellationToken);
        if (existingAlliance == null)
        {
            logger.LogInformation("Alliance with ID {AllianceId} not found", request.AllianceId);
            return [];
        }

        if (existingAlliance.AthRankings.Count == 0)
        {
            return [];
        }

        var eventIds = existingAlliance.AthRankings.Select(x => x.InGameEventId).ToHashSet();
        var events = await context.InGameEvents.Where(x => eventIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);
        
        return existingAlliance.AthRankings.Where(x => events.ContainsKey(x.InGameEventId))
            .Select(x => athRankingDtoFactory.Create(x, events[x.InGameEventId]))
            .ToList();
    }
}
