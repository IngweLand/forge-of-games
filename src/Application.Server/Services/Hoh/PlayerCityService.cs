using AutoMapper;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Providers;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.InnSdk.Hoh.Providers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class PlayerCityService(
    IInGameRawDataTableRepository inGameRawDataTableRepository,
    InGameRawDataTablePartitionKeyProvider partitionKeyProvider,
    IGameWorldsProvider gameWorldsProvider,
    IInGameDataParsingService inGameDataParsingService,
    IHohCoreDataRepository coreDataRepository,
    IHohCityFactory cityFactory,
    IMapper mapper,
    IInnSdkClient innSdkClient,
    ILogger<PlayerCityService> logger) : IPlayerCityService
{
    private static DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow);

    public async Task<HohCity?> GetCityAsync(string gameWorldId, int playerId, string playerName)
    {
        var gameWorld = gameWorldsProvider.GetGameWorlds().FirstOrDefault(config => config.Id == gameWorldId);
        if (gameWorld == null)
        {
            logger.LogWarning("GameWorld with ID {GameWorldId} not found.", gameWorldId);
            return null;
        }

        var storedCity = await GetStoredCityAsync(gameWorld.Id, playerId);

        if (storedCity == null)
        {
            var cityData = await FetchCityAsync(gameWorld, playerId);
            if (cityData == null)
            {
                return null;
            }

            storedCity = await SaveCityAsync(gameWorld, playerId, cityData);
        }

        if (storedCity == null)
        {
            return null;
        }

        return await CreateCity(mapper.Map<City>(storedCity), playerName);
    }

    private async Task<HohCity?> CreateCity(City cityDto, string playerName)
    {
        try
        {
            var buildings = await coreDataRepository.GetBuildingsAsync(cityDto.CityId);

            var cityName = $"{playerName} - {cityDto.CityId} - {DateTime.UtcNow:d}";
            return cityFactory.Create(cityDto, buildings.ToDictionary(b => b.Id), WonderId.Undefined, 0, cityName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create HohCity for cityId: {CityId}", cityDto.CityId);
        }

        return null;
    }

    private OtherCity? TryParseCity(InGameRawData rawData, string gameWorldId, int playerId)
    {
        try
        {
            return inGameDataParsingService.ParseOtherCity(rawData.Base64Data);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error parsing city raw data: {@Data}", new {Date = Today, gameWorldId, playerId});
            return null;
        }
    }

    private async Task<byte[]?> FetchCityAsync(GameWorldConfig gameWorld, int playerId)
    {
        try
        {
            return await innSdkClient.CityService.GetOtherCityRawDataAsync(gameWorld, playerId);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error fetching city: {@Data}", new {GameWorldId = gameWorld.Id, playerId});
        }

        return null;
    }

    private async Task<OtherCity?> SaveCityAsync(GameWorldConfig gameWorld, int playerId, byte[] data)
    {
        try
        {
            var now = DateTime.UtcNow;
            var rawData = new InGameRawData
            {
                Base64Data = Convert.ToBase64String(data),
                CollectedAt = now,
            };

            // Validate we indeed have a city
            var parsed = TryParseCity(rawData, gameWorld.Id, playerId);
            if (parsed == null)
            {
                return null;
            }

            var today = DateOnly.FromDateTime(now);
            var pk = partitionKeyProvider.OtherCity(gameWorld.Id, today);
            await inGameRawDataTableRepository.SaveAsync(rawData, pk, playerId.ToString());
            return parsed;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error saving city raw data: {@Data}.", new {GameWorldId = gameWorld.Id, playerId});
        }

        return null;
    }

    private async Task<OtherCity?> GetStoredCityAsync(string gameWorldId, int playerId)
    {
        var pk = partitionKeyProvider.OtherCity(gameWorldId, Today);
        var storedCity = await inGameRawDataTableRepository.GetAsync(pk, playerId.ToString());

        return storedCity != null ? TryParseCity(storedCity, gameWorldId, playerId) : null;
    }
}
