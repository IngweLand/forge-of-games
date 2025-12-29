using System.Text.Json;
using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Caching.Interfaces;
using Ingweland.Fog.Application.Client.Web.Constants;
using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Relics;
using Ingweland.Fog.Shared.Utils;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator;

public class EquipmentConfiguratorUiService(
    IEquipmentConfigurationProfileFactory equipmentConfigurationProfileFactory,
    IPersistenceService persistenceService,
    IHeroProfileUiService heroProfileUiService,
    IHeroEquipmentConfigurationViewModelFactory equipmentConfigurationViewModelFactory,
    IMapper mapper,
    IHohCoreDataCache coreDataCache,
    ILogger<EquipmentConfiguratorUiService> logger) : IEquipmentConfiguratorUiService
{
    private IReadOnlyDictionary<int, EquipmentItemViewModel> _equipmentItems = null!;
    private IReadOnlyDictionary<string, List<HeroEquipmentConfigurationViewModel>> _heroEquipment = null!;
    private IReadOnlyDictionary<string, HeroProfileIdentifier> _heroes = null!;
    private IReadOnlySet<string> _heroIds = null!;
    private EquipmentConfigurationProfile _profile = null!;

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

    public async Task<EquipmentConfigurationProfile?> InitializeAsync()
    {
        var compressed = await persistenceService.GetItemAsync<byte[]>(PersistenceKeys.EQUIPMENT_CONFIGURATION);
        if (compressed == null)
        {
            return null;
        }

        EquipmentConfigurationProfile? existing = null;
        try
        {
            existing = JsonSerializer.Deserialize<EquipmentConfigurationProfile>(
                CompressionUtils.DecompressToString(compressed));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to deserialize existing equipment configuration profile.");
        }

        if (existing != null)
        {
            _profile = existing;
            _heroIds = _profile.Heroes.Select(x => x.HeroId).ToHashSet();
            _heroes = _profile.Heroes.ToDictionary(x => x.HeroId);
            _equipmentItems = mapper.Map<IReadOnlyCollection<EquipmentItemViewModel>>(_profile.Equipment)
                .ToDictionary(x => x.Id);
            var equipmentData = await coreDataCache.GetEquipmentDataAsync();
            _heroEquipment = _profile.Configurations
                .GroupBy(x => x.HeroId)
                .ToDictionary(x => x.Key,
                    x => x.Select(y =>
                            equipmentConfigurationViewModelFactory.Create(y, _equipmentItems,
                                equipmentData.StatAttributes, equipmentData.Sets))
                        .ToList());
        }

        return existing;
    }

    public async Task<IReadOnlyCollection<HeroEquipmentViewModel>> GetHeroes(HeroFilterRequest request)
    {
        var heroes = await heroProfileUiService.GetHeroes(request, _heroIds);
        var results = new List<HeroEquipmentViewModel>();
        foreach (var hero in heroes)
        {
            results.Add(new HeroEquipmentViewModel
            {
                Hero = hero,
                Equipment = _heroEquipment.GetValueOrDefault(hero.Id, []),
            });
        }

        return results;
    }

    private async Task<EquipmentConfigurationProfile> AddProfileAsync(IReadOnlyCollection<HeroProfileIdentifier> heroes,
        IReadOnlyCollection<RelicItem> relics, IReadOnlyCollection<EquipmentItem> equipment,
        BarracksProfile barracksProfile)
    {
        var profile = equipmentConfigurationProfileFactory.Create(heroes, relics, equipment, barracksProfile);
        await SaveAsync(profile);
        return profile;
    }

    private async Task SaveAsync(EquipmentConfigurationProfile profile)
    {
        var serialized = JsonSerializer.Serialize(profile);
        await persistenceService.SetItemAsync(PersistenceKeys.EQUIPMENT_CONFIGURATION,
            CompressionUtils.CompressString(serialized));
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
            if (!heroIds.Contains(config.HeroId))
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

        var profile =
            equipmentConfigurationProfileFactory.Create(heroes, relics, equipment, barracksProfile, configurations);
        await SaveAsync(profile);
        return profile;
    }
}
