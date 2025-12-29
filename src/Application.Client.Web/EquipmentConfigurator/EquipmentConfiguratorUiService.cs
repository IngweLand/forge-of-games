using System.Collections.ObjectModel;
using Ingweland.Fog.Application.Client.Web.Caching.Interfaces;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;
using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.ViewModels;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator;

public class EquipmentConfiguratorUiService(
    IEquipmentProfilePersistenceService persistenceService,
    IHeroProfileUiService heroProfileUiService,
    IEquipmentConfigurationViewModelFactory equipmentConfigurationViewModelFactory,
    IEquipmentItemViewModel2Factory equipmentItemViewModelFactory,
    IHohCoreDataCache coreDataCache,
    IUnitStatFactory unitStatFactory,
    IHeroEquipmentConfigurationFactory heroEquipmentConfigurationFactory,
    IEquipmentSlotTypeIconUrlProvider equipmentSlotTypeIconUrlProvider,
    IAssetUrlProvider assetUrlProvider,
    IUnitStatsViewModelFactory unitStatsViewModelFactory,
    IEquipmentStatsCalculators equipmentStatsCalculators,
    ILogger<EquipmentConfiguratorUiService> logger) : IEquipmentConfiguratorUiService
{
    private static readonly List<EquipmentSlotType> SlotTypes =
    [
        EquipmentSlotType.Hand, EquipmentSlotType.Garment, EquipmentSlotType.Hat, EquipmentSlotType.Neck,
        EquipmentSlotType.Ring,
    ];

    private IReadOnlyDictionary<string, HeroEquipmentViewModel> _currentHeroEquipmentVms = null!;
    private IReadOnlyDictionary<int, EquipmentItem> _equipment;
    private IReadOnlyDictionary<int, EquipmentItemViewModel2> _equipmentViewModels = null!;
    private IReadOnlyDictionary<string, IReadOnlyDictionary<UnitStatType, float>> _heroBasicStats = null!;
    private IReadOnlyDictionary<string, List<EquipmentConfigurationViewModel>> _heroEquipment = null!;
    private IReadOnlySet<string> _heroIds = null!;

    public IReadOnlyDictionary<EquipmentSet, string> EquipmentSets { get; private set; } = null!;
    public EquipmentProfile Profile { get; private set; }
    public IReadOnlyDictionary<StatAttribute, string> StatAttributes { get; private set; } = null!;

    public async Task<IReadOnlyCollection<HeroEquipmentViewModel>> GetHeroes(HeroFilterRequest request)
    {
        var heroes = await heroProfileUiService.GetHeroes(request, _heroIds);
        var results = new List<HeroEquipmentViewModel>();
        foreach (var hero in heroes)
        {
            results.Add(new HeroEquipmentViewModel
            {
                Hero = hero,
                Equipment = new ObservableCollection<EquipmentConfigurationViewModel>(
                    _heroEquipment.GetValueOrDefault(hero.Id, []).OrderByDescending(x => x.IsInGame)),
            });
        }

        _currentHeroEquipmentVms = results.ToDictionary(x => x.Hero.Id);
        return results;
    }

    public async Task<EquipmentConfigurationViewModel> CreateEquipmentConfigurationAsync(
        HeroEquipmentViewModel heroEquipment)
    {
        var newConfig = heroEquipmentConfigurationFactory.Create(heroEquipment.Hero.Id);
        Profile.Configurations.Add(newConfig);
        await persistenceService.SaveAsync(Profile);

        var newConfigVm = equipmentConfigurationViewModelFactory.Create(newConfig.Id, heroEquipment.Hero);
        _heroEquipment[heroEquipment.Hero.Id].Add(newConfigVm);
        heroEquipment.Equipment.Add(newConfigVm);
        return newConfigVm;
    }

    public IReadOnlyCollection<EquipmentItemViewModel2> GetEquipment(EquipmentSlotType slot,
        EquipmentFilterRequest request)
    {
        var all = _equipmentViewModels.Values.Where(x => x.EquipmentItem.EquipmentSlotType == slot);
        if (!request.IsEmpty())
        {
            if (request.Sets.Count > 0)
            {
                all = all.Where(x => request.Sets.Contains(x.EquipmentItem.EquipmentSet));
            }

            if (request.Rarities.Count > 0)
            {
                all = all.Where(x => request.Rarities.Contains(x.EquipmentItem.EquipmentRarity));
            }

            if (request.HideEquipped)
            {
                all = all.Where(x => x.EquippedOnHeroes.Count == 0);
            }

            if (request.MainAttributes.Count > 0)
            {
                all = all.Where(x => request.MainAttributes.Contains(x.EquipmentItem.MainAttribute.StatAttribute));
            }

            if (request.SubAttributes.Count > 0)
            {
                all = all.Where(x =>
                {
                    var subAttributes = request.OnlyUnlockedSubAttributes
                        ? x.EquipmentItem.SubAttributes.Where(y => y.Unlocked)
                        : x.EquipmentItem.SubAttributes;
                    return subAttributes.Any(y => request.SubAttributes.Contains(y.StatAttribute));
                });
            }
        }

        return all
            .OrderByDescending(x => x.EquipmentItem.Level)
            .ThenByDescending(x => x.EquipmentItem.EquipmentRarity)
            .ToList();
    }

    public async Task<EquipmentConfigurationViewModel> DuplicateEquipmentConfigurationAsync(
        string equipmentConfigurationId)
    {
        var existingEquipmentConfiguration = Profile.Configurations.First(x => x.Id == equipmentConfigurationId);
        var newConfig = heroEquipmentConfigurationFactory.Duplicate(existingEquipmentConfiguration);
        Profile.Configurations.Add(newConfig);
        _ = Task.Run(async () => await persistenceService.SaveAsync(Profile));
        var hero = await heroProfileUiService.GetHeroBasicsAsync(newConfig.HeroId);
        var equipmentData = await coreDataCache.GetEquipmentDataAsync();
        var newConfigVm = equipmentConfigurationViewModelFactory.Create(newConfig, hero!,
            _heroBasicStats[newConfig.HeroId],
            _equipmentViewModels, equipmentData.SetDefinitions, equipmentData.UnitStatNames);
        _heroEquipment[newConfig.HeroId].Add(newConfigVm);
        if (_currentHeroEquipmentVms.TryGetValue(newConfig.HeroId, out var heroEquipment))
        {
            heroEquipment.Equipment.Add(newConfigVm);
        }

        return newConfigVm;
    }

    public async Task<bool> InitializeAsync(string profileId)
    {
        var existing = await persistenceService.GetAsync(profileId);
        if (existing == null)
        {
            return false;
        }

        Profile = existing;
        _heroIds = Profile.Heroes.Select(x => x.HeroId).ToHashSet();
        _heroBasicStats = await CreateHeroBasicStats(Profile.Heroes);
        _equipment = Profile.Equipment.ToDictionary(x => x.Id);
        var equipmentData = await coreDataCache.GetEquipmentDataAsync();
        var heroes = (await heroProfileUiService.GetHeroes()).ToDictionary(x => x.Id);
        var equipmentWithHeroesMap = new Dictionary<int, HashSet<string>>();
        foreach (var ec in Profile.Configurations)
        {
            foreach (var equipmentId in ec.GetIds())
            {
                if (!equipmentWithHeroesMap.TryGetValue(equipmentId, out var heroIds))
                {
                    heroIds = [];
                    equipmentWithHeroesMap.Add(equipmentId, heroIds);
                }

                heroIds.Add(ec.HeroId);
            }
        }

        _equipmentViewModels = _equipment
            .Select(x =>
            {
                var equippedOnHeroes = equipmentWithHeroesMap.GetValueOrDefault(x.Key, []).Select(y => heroes[y]);
                return equipmentItemViewModelFactory.CreateItem(x.Value, equippedOnHeroes, equipmentData.SetNames,
                    equipmentData.StatAttributeNames);
            })
            .ToDictionary(x => x.Id);

        _heroEquipment = Profile.Configurations
            .GroupBy(x => x.HeroId)
            .ToDictionary(x => x.Key,
                x => x.Select(y =>
                    equipmentConfigurationViewModelFactory.Create(y, heroes[y.HeroId], _heroBasicStats[y.HeroId],
                        _equipmentViewModels, equipmentData.SetDefinitions, equipmentData.UnitStatNames)).ToList());

        EquipmentSets = Enum.GetValues<EquipmentSet>().Where(x => x != EquipmentSet.Undefined)
            .ToDictionary(x => x, x => equipmentData.SetNames.GetValueOrDefault(x.ToString(), x.ToString()));

        StatAttributes = Enum.GetValues<StatAttribute>()
            .ToDictionary(x => x, x => equipmentData.StatAttributeNames.GetValueOrDefault(x, x.ToString()));

        return true;
    }

    public void DeleteEquipmentConfiguration(EquipmentConfigurationViewModel equipmentConfiguration)
    {
        for (var i = 0; i < Profile.Configurations.Count; i++)
        {
            var x = Profile.Configurations[i];
            if (x.Id == equipmentConfiguration.Id)
            {
                Profile.Configurations.RemoveAt(i);
                break;
            }
        }

        _ = Task.Run(async () => await persistenceService.SaveAsync(Profile));
        _heroEquipment[equipmentConfiguration.Hero.Id].Remove(equipmentConfiguration);
        if (_currentHeroEquipmentVms.TryGetValue(equipmentConfiguration.Hero.Id, out var heroEquipment))
        {
            heroEquipment.Equipment.Remove(equipmentConfiguration);
        }
    }

    public async Task<IReadOnlyCollection<EquipmentSlotWithSetViewModel>> GetEquipmentSlotTypesAsync(
        EquipmentConfigurationViewModel equipmentConfiguration)
    {
        var equipmentData = await coreDataCache.GetEquipmentDataAsync();

        return SlotTypes.Select(x =>
        {
            if (!equipmentData.SlotTypeNames.TryGetValue(x, out var name))
            {
                name = x.ToString();
            }

            var set = equipmentConfiguration.Get(x);
            return new EquipmentSlotWithSetViewModel
            {
                SlotType = x,
                Name = name,
                IconUrl = equipmentSlotTypeIconUrlProvider.GetIconUrl(x),
                SetIconUrl = set != null
                    ? assetUrlProvider.GetHohEquipmentSetIconUrl(set.EquipmentItem.EquipmentSet)
                    : null,
            };
        }).ToList();
    }

    public async Task SetEquipmentAsync(EquipmentConfigurationViewModel equipmentConfiguration,
        EquipmentSlotType slot, EquipmentItemViewModel2? equipment)
    {
        var configuration = Profile.Configurations.First(x => x.Id == equipmentConfiguration.Id);
        switch (slot)
        {
            case EquipmentSlotType.Hand:
            {
                equipmentConfiguration.Hand?.EquippedOnHeroes.Remove(equipmentConfiguration.Hero);

                configuration.HandId = equipment?.Id;
                equipmentConfiguration.Hand = equipment;
                break;
            }
            case EquipmentSlotType.Garment:
            {
                equipmentConfiguration.Garment?.EquippedOnHeroes.Remove(equipmentConfiguration.Hero);

                configuration.GarmentId = equipment?.Id;
                equipmentConfiguration.Garment = equipment;
                break;
            }
            case EquipmentSlotType.Hat:
            {
                equipmentConfiguration.Hat?.EquippedOnHeroes.Remove(equipmentConfiguration.Hero);

                configuration.HatId = equipment?.Id;
                equipmentConfiguration.Hat = equipment;
                break;
            }
            case EquipmentSlotType.Neck:
            {
                equipmentConfiguration.Neck?.EquippedOnHeroes.Remove(equipmentConfiguration.Hero);

                configuration.NeckId = equipment?.Id;
                equipmentConfiguration.Neck = equipment;
                break;
            }
            case EquipmentSlotType.Ring:
            {
                equipmentConfiguration.Ring?.EquippedOnHeroes.Remove(equipmentConfiguration.Hero);

                configuration.RingId = equipment?.Id;
                equipmentConfiguration.Ring = equipment;
                break;
            }
        }

        if (equipment != null)
        {
            equipment.EquippedOnHeroes.Add(equipmentConfiguration.Hero);
        }

        var equipmentData = await coreDataCache.GetEquipmentDataAsync();
        var boostedStats = equipmentStatsCalculators.Calculate(equipmentConfiguration.Hand?.EquipmentItem,
            equipmentConfiguration.Garment?.EquipmentItem, equipmentConfiguration.Hat?.EquipmentItem,
            equipmentConfiguration.Neck?.EquipmentItem, equipmentConfiguration.Ring?.EquipmentItem,
            _heroBasicStats[configuration.HeroId], equipmentData.SetDefinitions);
        equipmentConfiguration.Stats =
            unitStatsViewModelFactory.CreateStatsItems(boostedStats, equipmentData.UnitStatNames);
        _ = Task.Run(async () => await persistenceService.SaveAsync(Profile));
    }

    public Task UpdateProfileNameAsync(string profileName)
    {
        Profile.Name = profileName;
        return persistenceService.SaveAsync(Profile);
    }

    public async Task<HeroEquipmentViewModel> GetHeroEquipmentConfiguration(string equipmentConfigurationId)
    {
        foreach (var kvp in _heroEquipment)
        {
            foreach (var ec in kvp.Value)
            {
                if (ec.Id != equipmentConfigurationId)
                {
                    continue;
                }

                var hero = await heroProfileUiService.GetHeroBasicsAsync(kvp.Key);
                if (hero == null)
                {
                    throw new InvalidOperationException($"Hero with id {kvp.Key} not found.");
                }

                return new HeroEquipmentViewModel
                {
                    Hero = hero,
                    Equipment = new ObservableCollection<EquipmentConfigurationViewModel>([ec]),
                };
            }
        }

        throw new InvalidOperationException($"Equipment configuration with id {equipmentConfigurationId} not found.");
    }

    private async Task<IReadOnlyDictionary<string, IReadOnlyDictionary<UnitStatType, float>>> CreateHeroBasicStats(
        IReadOnlyCollection<HeroProfileIdentifier> heroes)
    {
        var result = new Dictionary<string, IReadOnlyDictionary<UnitStatType, float>>();
        foreach (var profileIdentifier in heroes)
        {
            var hero = await coreDataCache.GetHeroAsync(profileIdentifier.HeroId);
            if (hero == null)
            {
                throw new InvalidOperationException($"Hero with id {profileIdentifier.HeroId} not found.");
            }

            var stats = unitStatFactory.CreateHeroStats(hero, profileIdentifier.Level,
                profileIdentifier.AscensionLevel);
            result.Add(profileIdentifier.HeroId, stats);
        }

        return result;
    }
}
