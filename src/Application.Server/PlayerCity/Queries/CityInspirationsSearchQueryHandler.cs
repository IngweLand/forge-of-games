using AutoMapper;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.PlayerCity;
using Ingweland.Fog.Models.Fog.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.PlayerCity.Queries;

public record CityInspirationsSearchQuery(CityInspirationsSearchRequest Request)
    : IRequest<IReadOnlyCollection<PlayerCitySnapshotBasicDto>>, ICacheableRequest
{
    public string CacheKey => $"CityInspirationsSearch_{Request.CityId}_{Request.AgeId}_{Request.SearchPreference}_{
        Request.AllowPremiumEntities}_{Request.OpenedExpansionsHash}";

    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class CityInspirationsSearchQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<CityInspirationsSearchQuery,
        IReadOnlyCollection<PlayerCitySnapshotBasicDto>>
{
    public async Task<IReadOnlyCollection<PlayerCitySnapshotBasicDto>> Handle(CityInspirationsSearchQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.PlayerCitySnapshots.AsNoTracking()
            .Include(x => x.Player)
            .Where(x => x.CityId == request.Request.CityId && x.AgeId == request.Request.AgeId);

        if (!string.IsNullOrWhiteSpace(request.Request.OpenedExpansionsHash))
        {
            query = query.Where(x => x.OpenedExpansionsHash == request.Request.OpenedExpansionsHash);
        }

        if (request.Request.AllowPremiumEntities)
        {
            query = query.Where(x => x.HasPremiumBuildings || !x.HasPremiumBuildings);
        }
        else
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
        var result = await query.Take(FogConstants.MaxPlayerCitySnapshotSearchResults)
            .ToListAsync(cancellationToken);

        var deduplicated = result.DistinctBy(x => new
        {
            x.CityId,
            x.AgeId,
            x.OpenedExpansionsHash,
            x.HasPremiumBuildings,
            x.Coins,
            x.Food,
            x.Goods,
        });
        return mapper.Map<IReadOnlyCollection<PlayerCitySnapshotBasicDto>>(deduplicated);
    }
}
