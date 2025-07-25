using AutoMapper;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.PlayerCity;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Shared.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.PlayerCity.Queries;

public record CityInspirationsSearchQuery(CityInspirationsSearchRequest Request)
    : IRequest<IReadOnlyCollection<PlayerCitySnapshotBasicDto>>, ICacheableRequest
{
    public string CacheKey => $"CityInspirationsSearch_{Request.CityId}_{Request.AgeId}_{Request.SearchPreference}_{
        Request.AllowPremiumEntities}_{Request.OpenedExpansionsHash}_{Request.TotalArea}";

    public TimeSpan? Duration { get; }
    public DateTimeOffset? Expiration => DateTimeUtils.GetNextMidnightUtc();
}

public class CityInspirationsSearchQueryHandler(
    IFogDbContext context,
    IMapper mapper,
    ILogger<CityInspirationsSearchQueryHandler> logger)
    : IRequestHandler<CityInspirationsSearchQuery,
        IReadOnlyCollection<PlayerCitySnapshotBasicDto>>
{
    public async Task<IReadOnlyCollection<PlayerCitySnapshotBasicDto>> Handle(CityInspirationsSearchQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Starting city inspirations search for CityId: {CityId}, AgeId: {AgeId}, SearchPreference: {SearchPreference}",
            request.Request.CityId, request.Request.AgeId, request.Request.SearchPreference);

        var initQuery = context.PlayerCitySnapshots.AsNoTracking()
            .Where(x => x.CityId == request.Request.CityId && x.AgeId == request.Request.AgeId);

        var query = initQuery;
        if (!string.IsNullOrWhiteSpace(request.Request.OpenedExpansionsHash))
        {
            logger.LogDebug("Filtering by OpenedExpansionsHash: {OpenedExpansionsHash}",
                request.Request.OpenedExpansionsHash);
            query = initQuery.Where(x => x.OpenedExpansionsHash == request.Request.OpenedExpansionsHash);
        }

        var result = await BuildQuery(query, request).ToListAsync(cancellationToken);
        logger.LogDebug("Initial query returned {Count} results", result.Count);

        if (result.Count == 0 && !string.IsNullOrWhiteSpace(request.Request.OpenedExpansionsHash))
        {
            logger.LogDebug("No results found with OpenedExpansionsHash, falling back to TotalArea filter: {TotalArea}",
                request.Request.TotalArea);
            query = initQuery.Where(x => x.TotalArea == request.Request.TotalArea);
            result = await BuildQuery(query, request).ToListAsync(cancellationToken);
            logger.LogDebug("TotalArea fallback query returned {Count} results", result.Count);
        }

        if (result.Count == 0)
        {
            logger.LogInformation("No city inspirations found for the search criteria");
            return [];
        }

        var distinctIds = result
            .GroupBy(x => x.PlayerId)
            .Select(g => g.First())
            .Take(FogConstants.MaxPlayerCitySnapshotSearchResults)
            .Select(x => x.Id)
            .ToList();

        var finalResult = await context.PlayerCitySnapshots.AsNoTracking()
            .Include(x => x.Player)
            .Where(x => distinctIds.Contains(x.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        var mappedResults = mapper.Map<IReadOnlyCollection<PlayerCitySnapshotBasicDto>>(finalResult);
        logger.LogInformation("Returning {Count} city inspirations", mappedResults.Count);

        return mappedResults;
    }

    private IQueryable<SearchResult> BuildQuery(IQueryable<PlayerCitySnapshot> query,
        CityInspirationsSearchQuery request)
    {
        if (!request.Request.AllowPremiumEntities)
        {
            logger.LogDebug("Filtering out premium buildings");
            query = query.Where(x => !x.HasPremiumBuildings);
        }

        logger.LogDebug("Ordering by search preference: {SearchPreference}", request.Request.SearchPreference);
        query = request.Request.SearchPreference switch
        {
            CitySnapshotSearchPreference.Goods => query.OrderByDescending(x => x.Goods),
            CitySnapshotSearchPreference.Coins => query.OrderByDescending(x => x.Coins),
            CitySnapshotSearchPreference.Food => query.OrderByDescending(x => x.Food),
            _ => query.OrderByDescending(x => x.Food),
        };

        return query.Select(x => new SearchResult(x.Id, x.PlayerId));
    }

    private record SearchResult(int Id, int PlayerId);
}
