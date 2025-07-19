using System.Text.Json;
using AutoMapper;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Infrastructure.Entities;
using Ingweland.Fog.Infrastructure.Repositories.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Infrastructure.Repositories;

public class InGameStartupDataRepository(
    ITableStorageRepository<InGameStartupDataTableEntity> tableStorageRepository,
    IMapper mapper,
    ILogger<InGameStartupDataRepository> logger) : IInGameStartupDataRepository
{
    private static readonly string PartitionKey = "inGameData";

    public async Task<InGameStartupData?> GetAsync(string inGameStartupDataId)
    {
        logger.LogInformation("Retrieving saved data: {Id}", inGameStartupDataId);

        try
        {
            var entity =
                await tableStorageRepository.GetAsync(PartitionKey, inGameStartupDataId);
            if (entity == null)
            {
                logger.LogInformation("Could not find entity by id: {Id}", inGameStartupDataId);
                return null;
            }

            logger.LogInformation("Successfully retrieved saved data {Id}", inGameStartupDataId);
            return entity.InGameStartupData;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve saved data {Id}", inGameStartupDataId);
            throw;
        }
    }

    public async Task<string> SaveAsync(InGameStartupData data)
    {
        logger.LogInformation("Starting to save startup data");

        var rowKey = Guid.NewGuid().ToString("N");
        var entity = new InGameStartupDataTableEntity
        {
            PartitionKey = PartitionKey,
            RowKey = rowKey,
            InGameStartupData = data,
            CollectedAt = DateTime.UtcNow,
        };

        try
        {
            await tableStorageRepository.AddAsync(entity);
            logger.LogInformation("Successfully saved startup data");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save startup data");
            throw;
        }

        return rowKey;
    }
}
