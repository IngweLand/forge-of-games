using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Infrastructure.Entities;
using Ingweland.Fog.Infrastructure.Repositories.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Infrastructure.Repositories;

public class HohCityRepository(
    ITableStorageRepository<HohCityTableEntity> tableStorageRepository,
    ILogger<HohCityRepository> logger) : IHohCityRepository
{
    private static readonly string PartitionKey = "city";

    public async Task<HohCity?> GetAsync(string cityId)
    {
        logger.LogInformation("Retrieving saved city: {CityId}", cityId);

        try
        {
            var entity =
                await tableStorageRepository.GetAsync(PartitionKey, cityId);
            if (entity == null)
            {
                logger.LogInformation("Could not find entity by id: {EntityId}", cityId);
                return null;
            }

            logger.LogInformation("Successfully retrieved city {EntityId}", cityId);
            return entity.City;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve saved city {EntityId}", cityId);
            throw;
        }
    }

    public async Task<string> SaveAsync(HohCity city)
    {
        logger.LogInformation("Starting to save city: {@CitySummary}", new {city.Id, city.Name});

        var rowKey = Guid.NewGuid().ToString("N");
        var entity = new HohCityTableEntity()
        {
            PartitionKey = PartitionKey,
            RowKey = rowKey,
            City = city,
        };

        try
        {
            await tableStorageRepository.AddAsync(entity);
            logger.LogInformation("Successfully saved city. {CityId} -> {RowKey}", city.Id, rowKey);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save city {CityId}", city.Id);
            throw;
        }

        return rowKey;
    }
}
