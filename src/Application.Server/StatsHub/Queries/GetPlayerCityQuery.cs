using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetPlayerCityQuery(int PlayerId) : IRequest<HohCity?>;

public class GetPlayerCityQueryHandler(
    IFogDbContext context,
    IPlayerCityService playerCityService,
    IHohCoreDataRepository coreDataRepository,
    IDataParsingService dataParsingService,
    IHohCityFactory cityFactory)
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
            return await CreateCity(existingCity, player.Name);
        }

        var fetchedCity = await playerCityService.FetchCityAsync(player.WorldId, player.InGamePlayerId, CityId.Capital);
        if (fetchedCity == null)
        {
            return null;
        }

        var savedCity = await playerCityService.SaveCityAsync(player.Id, fetchedCity);
        if (savedCity == null)
        {
            return null;
        }

        return await CreateCity(savedCity, player.Name);
    }

    private async Task<HohCity> CreateCity(PlayerCitySnapshot citySnapshot, string playerName)
    {
        var otherCity = dataParsingService.ParseOtherCity(citySnapshot.Data);
        var buildings = await coreDataRepository.GetBuildingsAsync(CityId.Capital);

        var cityName = $"{playerName} - {otherCity.CityId} - {DateTime.UtcNow:d}";
        return cityFactory.Create(otherCity, buildings.ToDictionary(b => b.Id), WonderId.Undefined, 0, cityName);
    }
}
