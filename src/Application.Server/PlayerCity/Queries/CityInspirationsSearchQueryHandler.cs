using AutoMapper;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.PlayerCity;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.PlayerCity.Queries;

public record CityInspirationsSearchQuery(CityInspirationsSearchRequest Request)
    : IRequest<IReadOnlyCollection<PlayerCitySnapshotBasicDto>>, ICacheableRequest
{
    public string CacheKey => $"CityInspirationsSearch_{Request.CityId}_{Request.AgeId}_{Request.SearchPreference}_{
        Request.AllowPremiumEntities}_{Request.OpenedExpansionsHash}_{Request.TotalArea}";

    public TimeSpan? Duration => TimeSpan.FromHours(6);
    public DateTimeOffset? Expiration { get; }
}

public class CityInspirationsSearchQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<CityInspirationsSearchQuery,
        IReadOnlyCollection<PlayerCitySnapshotBasicDto>>
{
    public async Task<IReadOnlyCollection<PlayerCitySnapshotBasicDto>> Handle(CityInspirationsSearchQuery request,
        CancellationToken cancellationToken)
    {
        var initQuery = context.PlayerCitySnapshots.AsNoTracking()
            .Include(x => x.Player)
            .Where(x => x.CityId == request.Request.CityId && x.AgeId == request.Request.AgeId);

        var query = initQuery;
        if (!string.IsNullOrWhiteSpace(request.Request.OpenedExpansionsHash))
        {
            query = initQuery.Where(x => x.OpenedExpansionsHash == request.Request.OpenedExpansionsHash);
        }

        var result = await BuildQuery(query, request).ToListAsync(cancellationToken);

        if (result.Count == 0 && !string.IsNullOrWhiteSpace(request.Request.OpenedExpansionsHash))
        {
            query = initQuery.Where(x => x.TotalArea == request.Request.TotalArea);
            result = await BuildQuery(query, request).ToListAsync(cancellationToken);
        }

        if (result.Count == 0)
        {
            return [];
        }

        var distinct = result
            .GroupBy(x => x.PlayerId)
            .Select(g => g.OrderByDescending(x => x.CollectedAt).First())
            .Take(FogConstants.MaxPlayerCitySnapshotSearchResults);

        return mapper.Map<IReadOnlyCollection<PlayerCitySnapshotBasicDto>>(distinct);
    }

    private IQueryable<PlayerCitySnapshot> BuildQuery(IQueryable<PlayerCitySnapshot> query,
        CityInspirationsSearchQuery request)
    {
        if (!request.Request.AllowPremiumEntities)
        {
            query = query.Where(x => !x.HasPremiumBuildings);
        }

        query = request.Request.SearchPreference switch
        {
            CitySnapshotSearchPreference.Goods => query.OrderByDescending(x => x.Goods),
            CitySnapshotSearchPreference.Coins => query.OrderByDescending(x => x.Coins),
            CitySnapshotSearchPreference.Food => query.OrderByDescending(x => x.Food),
            _ => query.OrderByDescending(x => x.Food),
        };

        return query;
    }
}
