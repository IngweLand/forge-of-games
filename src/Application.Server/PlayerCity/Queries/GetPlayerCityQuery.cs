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

        var existingCity = await GetCityAsync(player.Id, player.Name, CityId.Capital, DateTime.UtcNow.ToDateOnly());
        if (existingCity != null)
        {
            return existingCity;
        }

        if (failedFetchesCache.IsFailedFetch(player.Key))
        {
            return null;
        }

        var fetchedCity = await playerCityService.FetchCityAsync(player.WorldId, player.InGamePlayerId);
        if (fetchedCity == null)
        {
            failedFetchesCache.AddFailedFetch(player.Key);
            return await GetCityAsync(player.Id, player.Name, CityId.Capital, null);
        }

        var savedCity = await playerCityService.SaveCityAsync(player.Id, fetchedCity);
        if (savedCity == null)
        {
            return await GetCityAsync(player.Id, player.Name, CityId.Capital, null);
        }

        return await cityCreationService.Create(savedCity, player.Name);
    }

    private async Task<HohCity?> GetCityAsync(int playerId, string playerName, CityId cityId, DateOnly? date)
    {
        var existingCity = await playerCityService.GetCityAsync(playerId, cityId, date);
        if (existingCity != null)
        {
            return await cityCreationService.Create(existingCity, playerName);
        }

        return null;
    }
}
