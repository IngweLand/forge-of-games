using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Shared.Helpers.Interfaces;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Client.Web.Services;

public class FogSharingUiService(
    IProtobufSerializer protobufSerializer,
    IFogSharingService sharingService,
    IPersistenceService persistenceService,
    IEquipmentProfilePersistenceService equipmentProfilePersistenceService,
    IEquipmentProfileFactory equipmentProfileFactory,
    ILogger<FogSharingUiService> logger)
    : IFogSharingUiService
{
    public Task<CreatedShareDto> ShareAsync(SharedDataDto data)
    {
        return sharingService.ShareAsync(data);
    }

    public SharedDataDto CreateSharedData(CityStrategy cityStrategy)
    {
        var bytes = protobufSerializer.SerializeToBytes(cityStrategy);
        return new SharedDataDto {Data = Convert.ToBase64String(bytes)};
    }

    public SharedDataDto CreateSharedData(EquipmentProfile equipmentProfile)
    {
        var bytes = protobufSerializer.SerializeToBytes(equipmentProfile);
        return new SharedDataDto {Data = Convert.ToBase64String(bytes)};
    }

    public async Task<bool> LoadCityStrategyAsync(string shareId)
    {
        var data = await sharingService.GetAsync(shareId);
        if (data == null)
        {
            return false;
        }

        try
        {
            var strategy = protobufSerializer.DeserializeFromBytes<CityStrategy>(Convert.FromBase64String(data.Data));
            strategy.Id = Guid.NewGuid().ToString();
            strategy.UpdatedAt = DateTime.Now;
            await persistenceService.SaveCityStrategy(strategy);
            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to load city strategy.");
            return false;
        }
    }

    public async Task<bool> LoadEquipmentProfileAsync(string shareId)
    {
        var data = await sharingService.GetAsync(shareId);
        if (data == null)
        {
            return false;
        }

        try
        {
            var loadedProfile =
                protobufSerializer.DeserializeFromBytes<EquipmentProfile>(Convert.FromBase64String(data.Data));
            var profile = equipmentProfileFactory.Create(loadedProfile.Name, loadedProfile.Heroes, loadedProfile.Relics,
                loadedProfile.Equipment, loadedProfile.BarracksProfile, loadedProfile.Configurations);
            await equipmentProfilePersistenceService.SaveAsync(profile);
            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to load equipment profile.");
            return false;
        }
    }
}
