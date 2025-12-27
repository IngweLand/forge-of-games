using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.PlayerCity.Abstractions;
using Ingweland.Fog.Dtos.Hoh.PlayerCity;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetPlayerCityPropertiesQuery(int PlayerId) : IRequest<PlayerCityPropertiesDto?>, ICacheableRequest
{
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class GetPlayerCityPropertiesQueryHandler(
    IPlayerCityService playerCityService,
    IFogDbContext context,
    IFailedPlayerCityFetchesCache failedFetchesCache,
    ILogger<GetPlayerCityPropertiesQueryHandler> logger)
    : IRequestHandler<GetPlayerCityPropertiesQuery, PlayerCityPropertiesDto?>
{
    public async Task<PlayerCityPropertiesDto?> Handle(GetPlayerCityPropertiesQuery request,
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

    private PlayerCityPropertiesDto CreateDto(PlayerCitySnapshot snapshot)
    {
        var totalPremiumExpansionCost =
            HohConstants.CapitalPremiumExpansionCost.Take(snapshot.PremiumExpansionCount).Sum();
        if (snapshot.PremiumExpansionCount > HohConstants.CapitalPremiumExpansionCost.Length)
        {
            logger.LogWarning(
                "Player {PlayerId} has {Count} premium expansions, which exceeds the known cost constants length of {Max}",
                snapshot.PlayerId, snapshot.PremiumExpansionCount, HohConstants.CapitalPremiumExpansionCost.Length);
        }

        return new PlayerCityPropertiesDto
        {
            Coins = snapshot.Coins24H,
            Food = snapshot.Food24H,
            Goods = snapshot.Goods24H,
            PremiumExpansionCount = snapshot.PremiumExpansionCount,
            TotalPremiumExpansionCost = totalPremiumExpansionCost,
        };
    }
}
