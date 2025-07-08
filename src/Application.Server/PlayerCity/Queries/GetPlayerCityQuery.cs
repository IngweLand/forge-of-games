using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.PlayerCity.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Extensions;
using Ingweland.Fog.Shared.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.PlayerCity.Queries;

public record GetPlayerCityQuery(int PlayerId) : IRequest<HohCity?>, ICacheableRequest
{
    public string CacheKey => $"PlayerCity_{PlayerId}";
    public TimeSpan? Duration { get; }
    public DateTimeOffset? Expiration => DateTimeUtils.GetNextMidnightUtc();
}

public class GetPlayerCityQueryHandler(
    IFogDbContext context,
    IPlayerCityService playerCityService,
    IHohCityCreationService cityCreationService,
    IFailedPlayerCityFetchesCache failedFetchesCache)
    : IRequestHandler<GetPlayerCityQuery, HohCity?>
{
    public async Task<HohCity?> Handle(GetPlayerCityQuery request, CancellationToken cancellationToken)
    {
        var player = await context.Players.FirstOrDefaultAsync(p => p.Id == request.PlayerId, cancellationToken);
        if (player == null)
        {
            return null;
        }

        var existingCity =
            await playerCityService.GetCityAsync(player.Id, CityId.Capital, DateTime.UtcNow.ToDateOnly());
        if (existingCity != null)
        {
            return await cityCreationService.Create(existingCity, player.Name);
        }

        if (failedFetchesCache.IsFailedFetch(player.Key))
        {
            return null;
        }

        var fetchedCity = await playerCityService.FetchCityAsync(player.WorldId, player.InGamePlayerId);
        if (fetchedCity == null)
        {
            failedFetchesCache.AddFailedFetch(player.Key);
            return null;
        }

        var savedCity = await playerCityService.SaveCityAsync(player.Id, fetchedCity);
        if (savedCity == null)
        {
            return null;
        }

        return await cityCreationService.Create(savedCity, player.Name);
    }
}
