using AutoMapper;
using Blazored.LocalStorage;
using Ingweland.Fog.Application.Client.Web.Constants;
using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Relics;
using Ingweland.Fog.Shared.Helpers.Interfaces;
using Ingweland.Fog.Shared.Utils;

namespace Ingweland.Fog.WebApp.Client.Services;

public class EquipmentProfilePersistenceService(
    IEquipmentProfileFactory equipmentProfileFactory,
    IPersistenceService persistenceService,
    IProtobufSerializer protobufSerializer,
    ILocalStorageService localStorageService,
    IMapper mapper,
    ILogger<EquipmentProfilePersistenceService> logger) : IEquipmentProfilePersistenceService
{
    public async Task UpsertProfileAsync(string? profileId, string? profileName,
        IReadOnlyCollection<HeroProfileIdentifier> heroes, IReadOnlyCollection<RelicItem> relics,
        IReadOnlyCollection<EquipmentItem> equipment, BarracksProfile barracksProfile)
    {
        EquipmentProfile? existing = null;
        if (profileId != null)
        {
            var compressed = await persistenceService.GetItemAsync<byte[]>(GetProfileKey(profileId));

            if (compressed != null)
            {
                try
                {
                    existing = protobufSerializer.DeserializeFromBytes<EquipmentProfile>(
                        CompressionUtils.Decompress(compressed));
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Failed to deserialize existing equipment configuration profile.");
                }
            }
        }

        if (existing == null)
        {
            await AddProfileAsync(profileName, heroes, relics, equipment, barracksProfile);
        }
        else
        {
            await UpdateProfileAsync(existing, heroes, relics, equipment, barracksProfile);
        }
    }

    public ValueTask DeleteAsync(string profileId)
    {
        var key = GetProfileKey(profileId);
        return persistenceService.RemoveItemAsync(key);
    }

    public async ValueTask<IReadOnlyCollection<EquipmentProfileBasicData>> GetProfiles()
    {
        var keys = await localStorageService.KeysAsync();
        var profileKeys = keys.Where(s => s.StartsWith(PersistenceKeys.EQUIPMENT_PROFILE));
        var profiles = new List<EquipmentProfileBasicData>();
        foreach (var profileKey in profileKeys)
        {
            var profile = await DoLoadProfileAsync(profileKey);
            if (profile != null)
            {
                profiles.Add(mapper.Map<EquipmentProfileBasicData>(profile));
            }
        }

        return profiles.OrderByDescending(x => x.UpdatedAt).ToList();
    }

    public async Task SaveAsync(EquipmentProfile profile)
    {
        var serialized = protobufSerializer.SerializeToBytes(profile);
        await persistenceService.SetItemAsync(GetProfileKey(profile.Id), CompressionUtils.Compress(serialized));
    }

    public Task<EquipmentProfile?> GetAsync(string profileId)
    {
        return DoLoadProfileAsync(GetProfileKey(profileId));
    }

    private async Task<EquipmentProfile> AddProfileAsync(string? profileName,
        IReadOnlyCollection<HeroProfileIdentifier> heroes, IReadOnlyCollection<RelicItem> relics,
        IReadOnlyCollection<EquipmentItem> equipment, BarracksProfile barracksProfile)
    {
        var profile =
            equipmentProfileFactory.Create(profileName, heroes, relics, equipment, barracksProfile);
        await SaveAsync(profile);
        return profile;
    }

    private static string GetProfileKey(string id)
    {
        return $"{PersistenceKeys.EQUIPMENT_PROFILE}.{id}";
    }

    private async Task<EquipmentProfile?> DoLoadProfileAsync(string key)
    {
        var compressed = await persistenceService.GetItemAsync<byte[]>(key);
        if (compressed == null)
        {
            return null;
        }

        try
        {
            return protobufSerializer.DeserializeFromBytes<EquipmentProfile>(
                CompressionUtils.Decompress(compressed));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to deserialize existing equipment configuration profile.");
        }

        return null;
    }

    private async Task UpdateProfileAsync(EquipmentProfile existing,
        IReadOnlyCollection<HeroProfileIdentifier> heroes,
        IReadOnlyCollection<RelicItem> relics, IReadOnlyCollection<EquipmentItem> equipment,
        BarracksProfile barracksProfile)
    {
        var heroIds = heroes.Select(x => x.HeroId).ToHashSet();
        var equipmentIds = equipment.Select(x => x.Id).ToHashSet();
        var configurations = new List<HeroEquipmentConfiguration>();
        foreach (var config in existing.Configurations)
        {
            if (config.IsInGame || !heroIds.Contains(config.HeroId))
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

        var newConfigurations = equipmentProfileFactory.CreateConfigurations(equipment);
        configurations.AddRange(newConfigurations);
        existing.Configurations = configurations;
        await SaveAsync(existing);
    }
}
