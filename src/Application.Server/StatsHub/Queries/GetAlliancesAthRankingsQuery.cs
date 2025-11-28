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

public record GetAlliancesAthRankingsQuery : IRequest<PaginatedList<AllianceDto>>, ICacheableRequest
{
    public required TreasureHuntLeague League { get; init; }
    public int PageSize { get; init; }
    public int StartIndex { get; init; }
    public required string WorldId { get; init; }
    public TimeSpan? Duration => TimeSpan.FromHours(1);
    public DateTimeOffset? Expiration { get; }
}

public class GetAlliancesAthRankingsQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<GetAlliancesAthRankingsQuery, PaginatedList<AllianceDto>>
{
    public async Task<PaginatedList<AllianceDto>> Handle(GetAlliancesAthRankingsQuery request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var latestAthEvent = await context.InGameEvents.FirstOrDefaultAsync(
            x => x.DefinitionId == EventDefinitionId.TreasureHuntLeague && x.WorldId == request.WorldId &&
                x.StartAt <= now && x.EndAt >= now,
            cancellationToken);
        if (latestAthEvent == null)
        {
            latestAthEvent = await context.InGameEvents
                .Where(x => x.DefinitionId == EventDefinitionId.TreasureHuntLeague && x.WorldId == request.WorldId &&
                    x.StartAt <= now)
                .OrderByDescending(x => x.StartAt)
                .FirstOrDefaultAsync(cancellationToken);

            if (latestAthEvent == null)
            {
                return PaginatedList<AllianceDto>.Empty;
            }
        }

// TODO implement validator instead
        var pageSize = Math.Min(request.PageSize, FogConstants.MAX_ALLIANCES_ATH_RANKINGS);
        var topRankingsQuery = context.AllianceAthRankings
            .Where(x => x.League == request.League && x.InGameEventId == latestAthEvent.Id);

        var total = Math.Min(await topRankingsQuery.CountAsync(cancellationToken),
            FogConstants.MAX_ALLIANCES_ATH_RANKINGS);
        var topRankings = await topRankingsQuery
            .OrderByDescending(x => x.Points)
            .Skip(request.StartIndex)
            .Take(pageSize)
            .ToDictionaryAsync(x => x.AllianceId, cancellationToken);

        var allianceIds = topRankings.Select(x => x.Key).ToHashSet();
        var alliances = await context.Alliances
            .Where(x => allianceIds.Contains(x.Id))
            .ProjectTo<AllianceDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        var today = now.ToDateOnly();
        alliances.ForEach(x =>
        {
            x.RankingPoints = topRankings[x.Id].Points;
            x.UpdatedAt = today;
        });
        return new PaginatedList<AllianceDto>(
            alliances
                .OrderByDescending(x => x.RankingPoints)
                .Select((x, i) =>
                {
                    x.Rank = request.StartIndex + i + 1;
                    return x;
                })
                .ToList(),
            request.StartIndex, total);
    }
}
