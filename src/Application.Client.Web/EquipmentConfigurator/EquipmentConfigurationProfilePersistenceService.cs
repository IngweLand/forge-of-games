using System.Text.Json;
using Ingweland.Fog.Application.Client.Web.Constants;
using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Relics;
using Ingweland.Fog.Shared.Utils;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator;

public class EquipmentConfigurationProfilePersistenceService(
    IEquipmentConfigurationProfileFactory equipmentConfigurationProfileFactory,
    IPersistenceService persistenceService,
    ILogger<EquipmentConfigurationProfilePersistenceService> logger) : IEquipmentConfigurationProfilePersistenceService
{
    public async Task UpsertProfileAsync(IReadOnlyCollection<HeroProfileIdentifier> heroes,
        IReadOnlyCollection<RelicItem> relics, IReadOnlyCollection<EquipmentItem> equipment,
        BarracksProfile barracksProfile)
    {
        var compressed = await persistenceService.GetItemAsync<byte[]>(PersistenceKeys.EQUIPMENT_CONFIGURATION);
        EquipmentConfigurationProfile? existing = null;
        if (compressed != null)
        {
            try
            {
                existing = JsonSerializer.Deserialize<EquipmentConfigurationProfile>(
                    CompressionUtils.DecompressToString(compressed));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to deserialize existing equipment configuration profile.");
            }
        }

        if (existing == null)
        {
            await AddProfileAsync(heroes, relics, equipment, barracksProfile);
        }
        else
        {
            await UpdateProfileAsync(existing, heroes, relics, equipment, barracksProfile);
        }
    }

    public async Task<EquipmentConfigurationProfile> AddProfileAsync(IReadOnlyCollection<HeroProfileIdentifier> heroes,
        IReadOnlyCollection<RelicItem> relics, IReadOnlyCollection<EquipmentItem> equipment,
        BarracksProfile barracksProfile)
    {
        var profile = equipmentConfigurationProfileFactory.Create(heroes, relics, equipment, barracksProfile);
        await SaveAsync(profile);
        return profile;
    }

    public async Task SaveAsync(EquipmentConfigurationProfile profile)
    {
        var serialized = JsonSerializer.Serialize(profile);
        await persistenceService.SetItemAsync(PersistenceKeys.EQUIPMENT_CONFIGURATION,
            CompressionUtils.CompressString(serialized));
    }

    public async Task<EquipmentConfigurationProfile?> GetAsync()
    {
        var compressed = await persistenceService.GetItemAsync<byte[]>(PersistenceKeys.EQUIPMENT_CONFIGURATION);
        if (compressed == null)
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<EquipmentConfigurationProfile>(
                CompressionUtils.DecompressToString(compressed));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to deserialize existing equipment configuration profile.");
        }

        return null;
    }

    private async Task<EquipmentConfigurationProfile> UpdateProfileAsync(EquipmentConfigurationProfile existing,
        IReadOnlyCollection<HeroProfileIdentifier> heroes,
        IReadOnlyCollection<RelicItem> relics, IReadOnlyCollection<EquipmentItem> equipment,
        BarracksProfile barracksProfile)
    {
        var heroIds = heroes.Select(x => x.HeroId).ToHashSet();
        var equipmentIds = equipment.Select(x => x.Id).ToHashSet();
        var configurations = new List<HeroEquipmentConfiguration>();
        foreach (var config in existing.Configurations)
        {
            if (!heroIds.Contains(config.HeroId) || config.IsInGame)
            {
                continue;
            }

            if (config.HandId != null && !equipmentIds.Contains(config.HandId.Value))
            {
                config.HandId = null;
            }

            if (config.GarmentId != null && !equipmentIds.Contains(config.GarmentId.Value))
            {
                config.GarmentId = null;
            }

            if (config.HatId != null && !equipmentIds.Contains(config.HatId.Value))
            {
                config.HatId = null;
            }

            if (config.NeckId != null && !equipmentIds.Contains(config.NeckId.Value))
            {
                config.NeckId = null;
            }

            if (config.RingId != null && !equipmentIds.Contains(config.RingId.Value))
            {
                config.RingId = null;
            }

            configurations.Add(config);
        }

        var newProfile = equipmentConfigurationProfileFactory.Create(heroes, relics, equipment, barracksProfile);
        configurations.AddRange(newProfile.Configurations);
        var profile =
            equipmentConfigurationProfileFactory.Create(heroes, relics, equipment, barracksProfile, configurations);
        profile.Id = existing.Id;
        await SaveAsync(profile);
        return profile;
    }
}
