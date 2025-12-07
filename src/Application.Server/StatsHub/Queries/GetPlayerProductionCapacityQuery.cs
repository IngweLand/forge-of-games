using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.PlayerCity.Abstractions;
using Ingweland.Fog.Dtos.Hoh.PlayerCity;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetPlayerProductionCapacityQuery(int PlayerId) : IRequest<PlayerProductionCapacityDto?>, ICacheableRequest
{
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class GetPlayerProductionCapacityQueryHandler(
    IPlayerCityService playerCityService,
    IFogDbContext context,
    IFailedPlayerCityFetchesCache failedFetchesCache)
    : IRequestHandler<GetPlayerProductionCapacityQuery, PlayerProductionCapacityDto?>
{
    public async Task<PlayerProductionCapacityDto?> Handle(GetPlayerProductionCapacityQuery request,
        CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.ToDateOnly();
        var existingSnapshot = await context.PlayerCitySnapshots.FirstOrDefaultAsync(
            x => x.PlayerId == request.PlayerId && x.CityId == CityId.Capital && x.CollectedAt == today,
            cancellationToken);
        if (existingSnapshot != null)
        {
            return CreateDto(existingSnapshot);
        }

        var player = await context.Players.FindAsync(request.PlayerId, cancellationToken);
        if (player == null)
        {
            return null;
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

        var newSnapshot = await playerCityService.SaveCityAsync(player.Id, fetchedCity);
        if (newSnapshot == null)
        {
            return null;
        }

        return CreateDto(newSnapshot);
    }

    private static PlayerProductionCapacityDto CreateDto(PlayerCitySnapshot snapshot)
    {
        return new PlayerProductionCapacityDto
        {
            Coins = snapshot.Coins24H,
            Food = snapshot.Food24H,
            Goods = snapshot.Goods24H,
        };
    }
}
