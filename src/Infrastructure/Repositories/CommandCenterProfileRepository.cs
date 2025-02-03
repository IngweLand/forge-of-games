using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Infrastructure.Entities;
using Ingweland.Fog.Infrastructure.Repositories.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Infrastructure.Repositories;

public class CommandCenterProfileRepository(
    ITableStorageRepository<CcProfileTableEntity> tableStorageRepository,
    ILogger<CommandCenterProfileRepository> logger) : ICommandCenterProfileRepository
{
    private static readonly string CommandCenterProfilePk = "profile";

    public async Task<BasicCommandCenterProfile?> GetAsync(string profileId)
    {
        logger.LogInformation("Retrieving saved profile: {ProfileId}", profileId);

        try
        {
            var entity =
                await tableStorageRepository.GetAsync(CommandCenterProfilePk, profileId);
            if (entity == null)
            {
                logger.LogInformation("Could not find entity by id: {ProfileId}", profileId);
                return null;
            }

            logger.LogInformation("Successfully retrieved profile {ProfileId}", profileId);
            return entity.Profile;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve saved profile {ProfileId}", profileId);
            throw;
        }
    }

    public async Task<string> SaveAsync(BasicCommandCenterProfile profile)
    {
        logger.LogInformation("Starting to save profile: {@ProfileSummary}",
            new {profile.Id, profile.Name});

        var rowKey = Guid.NewGuid().ToString("N");
        var entity = new CcProfileTableEntity()
        {
            PartitionKey = CommandCenterProfilePk,
            RowKey = rowKey,
            Profile = profile,
        };

        try
        {
            await tableStorageRepository.AddAsync(entity);
            logger.LogInformation("Successfully saved profile. {ProfileId} -> {RowKey}",
                profile.Id, rowKey);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save profile {ProfileId}", profile.Id);
            throw;
        }

        return rowKey;
    }
}
