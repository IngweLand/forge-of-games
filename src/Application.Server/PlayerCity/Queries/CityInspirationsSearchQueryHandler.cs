using System.Linq.Expressions;
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
    public TimeSpan? Duration { get; }
    public DateTimeOffset? Expiration => DateTimeUtils.GetNextMidnightUtc();
}

public class CityInspirationsSearchQueryHandler(
    IFogDbContext context,
    ILogger<CityInspirationsSearchQueryHandler> logger)
    : IRequestHandler<CityInspirationsSearchQuery,
        IReadOnlyCollection<PlayerCitySnapshotBasicDto>>
{
    private static readonly
        Dictionary<(CityProductionMetric Metric, CitySnapshotSearchPreference Preference),
            Expression<Func<PlayerCitySnapshot, int>>> SortSelectors = new()
        {
            // Storage
            [(CityProductionMetric.Storage, CitySnapshotSearchPreference.Goods)] = x => x.Goods,
            [(CityProductionMetric.Storage, CitySnapshotSearchPreference.Coins)] = x => x.Coins,
            [(CityProductionMetric.Storage, CitySnapshotSearchPreference.Food)] = x => x.Food,

            // OneHour
            [(CityProductionMetric.OneHour, CitySnapshotSearchPreference.Goods)] = x => x.Goods1H,
            [(CityProductionMetric.OneHour, CitySnapshotSearchPreference.Coins)] = x => x.Coins1H,
            [(CityProductionMetric.OneHour, CitySnapshotSearchPreference.Food)] = x => x.Food1H,

            // OneDay
            [(CityProductionMetric.OneDay, CitySnapshotSearchPreference.Goods)] = x => x.Goods24H,
            [(CityProductionMetric.OneDay, CitySnapshotSearchPreference.Coins)] = x => x.Coins24H,
            [(CityProductionMetric.OneDay, CitySnapshotSearchPreference.Food)] = x => x.Food24H,

            // Storage per area
            [(CityProductionMetric.StoragePerCityArea, CitySnapshotSearchPreference.Goods)] = x => x.GoodsPerArea,
            [(CityProductionMetric.StoragePerCityArea, CitySnapshotSearchPreference.Coins)] = x => x.CoinsPerArea,
            [(CityProductionMetric.StoragePerCityArea, CitySnapshotSearchPreference.Food)] = x => x.FoodPerArea,

            // OneHour per area
            [(CityProductionMetric.OneHourPerCityArea, CitySnapshotSearchPreference.Goods)] = x => x.Goods1HPerArea,
            [(CityProductionMetric.OneHourPerCityArea, CitySnapshotSearchPreference.Coins)] = x => x.Coins1HPerArea,
            [(CityProductionMetric.OneHourPerCityArea, CitySnapshotSearchPreference.Food)] = x => x.Food1HPerArea,

            // OneDay per area
            [(CityProductionMetric.OneDayPerCityArea, CitySnapshotSearchPreference.Goods)] = x => x.Goods24HPerArea,
            [(CityProductionMetric.OneDayPerCityArea, CitySnapshotSearchPreference.Coins)] = x => x.Coins24HPerArea,
            [(CityProductionMetric.OneDayPerCityArea, CitySnapshotSearchPreference.Food)] = x => x.Food24HPerArea,
        };

    private static readonly Expression<Func<PlayerCitySnapshot, int>> DefaultSortSelector = x => x.Food;

    public async Task<IReadOnlyCollection<PlayerCitySnapshotBasicDto>> Handle(CityInspirationsSearchQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Starting city inspirations search for CityId: {CityId}, AgeId: {AgeId}, SearchPreference: {SearchPreference}",
            request.Request.CityId, request.Request.AgeId, request.Request.SearchPreference);

        var initQuery = context.PlayerCitySnapshots.AsNoTracking()
            .Where(x => x.CityId == request.Request.CityId && x.AgeId == request.Request.AgeId);

        var primaryQuery = initQuery;
        if (!string.IsNullOrWhiteSpace(request.Request.OpenedExpansionsHash))
        {
            logger.LogDebug("Filtering by OpenedExpansionsHash: {OpenedExpansionsHash}",
                request.Request.OpenedExpansionsHash);
            primaryQuery = initQuery.Where(x => x.OpenedExpansionsHash == request.Request.OpenedExpansionsHash);
        }

        var result = await BuildQuery(primaryQuery, request).ToListAsync(cancellationToken);
        logger.LogDebug("Initial query returned {Count} results", result.Count);

        if (result.Count == 0 && !string.IsNullOrWhiteSpace(request.Request.OpenedExpansionsHash))
        {
            logger.LogDebug("No results found with OpenedExpansionsHash, falling back to TotalArea filter: {TotalArea}",
                request.Request.TotalArea);
            var fallbackQuery = initQuery.Where(x => x.TotalArea == request.Request.TotalArea);
            result = await BuildQuery(fallbackQuery, request).ToListAsync(cancellationToken);
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

        finalResult = finalResult
            .OrderBy(x => distinctIds.IndexOf(x.Id))
            .ToList();

        var mappedResults = CreateDtos(request.Request.ProductionMetric, finalResult);

        logger.LogInformation("Returning {Count} city inspirations", mappedResults.Count);

        return mappedResults;
    }

    private static Func<PlayerCitySnapshot, int> GetSelectorFunc(CityProductionMetric productionMetric,
        CitySnapshotSearchPreference searchPreference)
    {
        return SortSelectors
            .TryGetValue((productionMetric, searchPreference), out var expr)
            ? expr.Compile()
            : DefaultSortSelector.Compile();
    }

    private static List<PlayerCitySnapshotBasicDto> CreateDtos(CityProductionMetric productionMetric,
        IEnumerable<PlayerCitySnapshot> snapshots)
    {
        return snapshots.Select(snapshot => new PlayerCitySnapshotBasicDto
        {
            Id = snapshot.Id,
            AgeId = snapshot.AgeId,
            CityId = snapshot.CityId,
            Coins = GetSelectorFunc(productionMetric, CitySnapshotSearchPreference.Coins)(snapshot),
            Food = GetSelectorFunc(productionMetric, CitySnapshotSearchPreference.Food)(snapshot),
            Goods = GetSelectorFunc(productionMetric, CitySnapshotSearchPreference.Goods)(snapshot),
            HappinessUsageRatio = snapshot.HappinessUsageRatio,
            HasPremiumHomeBuildings = snapshot.HasPremiumHomeBuildings,
            HasPremiumFarmBuildings = snapshot.HasPremiumFarmBuildings,
            HasPremiumCultureBuildings = snapshot.HasPremiumCultureBuildings,
            PlayerName = snapshot.Player.Name,
            TotalArea = snapshot.TotalArea,
        }).ToList();
    }

    private IQueryable<SearchResult> BuildQuery(IQueryable<PlayerCitySnapshot> query,
        CityInspirationsSearchQuery request)
    {
        if (!request.Request.AllowPremiumHomeBuildings)
        {
            query = query.Where(x => !x.HasPremiumHomeBuildings);
        }

        if (!request.Request.AllowPremiumFarmBuildings)
        {
            query = query.Where(x => !x.HasPremiumFarmBuildings);
        }

        if (!request.Request.AllowPremiumCultureBuildings)
        {
            query = query.Where(x => !x.HasPremiumCultureBuildings);
        }

        var key = (request.Request.ProductionMetric, request.Request.SearchPreference);
        if (!SortSelectors.TryGetValue(key, out var sortSelector))
        {
            logger.LogWarning(
                "Unknown metric/preference combination ({Metric}, {Preference}), using default sort",
                key.ProductionMetric, key.SearchPreference);

            sortSelector = DefaultSortSelector;
        }

        logger.LogDebug(
            "Ordering using selector for Metric={Metric}, Preference={Preference}",
            key.ProductionMetric, key.SearchPreference);
        query = query.OrderByDescending(sortSelector);
        return query.Select(x => new SearchResult(x.Id, x.PlayerId));
    }

    private record SearchResult(int Id, int PlayerId);
}
