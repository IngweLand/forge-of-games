using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Caching.Interfaces;
using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;
using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.ViewModels;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator;

public class EquipmentConfiguratorUiService(
    IEquipmentConfigurationProfilePersistenceService persistenceService,
    IHeroProfileUiService heroProfileUiService,
    IEquipmentConfigurationViewModelFactory equipmentConfigurationViewModelFactory,
    IEquipmentItemViewModel2Factory equipmentItemViewModelFactory,
    IMapper mapper,
    IHohCoreDataCache coreDataCache,
    ILogger<EquipmentConfiguratorUiService> logger) : IEquipmentConfiguratorUiService
{
    private IReadOnlyDictionary<int, EquipmentItem> _equipment;
    private IReadOnlyDictionary<int, EquipmentItemViewModel2> _equipmentViewModels = null!;
    private IReadOnlyDictionary<string, List<EquipmentConfigurationViewModel>> _heroEquipment = null!;
    private IReadOnlyDictionary<string, HeroProfileIdentifier> _heroes = null!;
    private IReadOnlySet<string> _heroIds = null!;
    private EquipmentConfigurationProfile _profile = null!;

    public async Task<EquipmentConfigurationProfile?> InitializeAsync()
    {
        var existing = await persistenceService.GetAsync();

        if (existing != null)
        {
            _profile = existing;
            _heroIds = _profile.Heroes.Select(x => x.HeroId).ToHashSet();
            _heroes = _profile.Heroes.ToDictionary(x => x.HeroId);
            _equipment = _profile.Equipment.ToDictionary(x => x.Id);
            var equipmentData = await coreDataCache.GetEquipmentDataAsync();
            _equipmentViewModels = _equipment
                .Select(x => equipmentItemViewModelFactory.CreateItem(x.Value, equipmentData.Sets,
                    equipmentData.StatAttributes))
                .ToDictionary(x => x.Id);

            _heroEquipment = _profile.Configurations
                .GroupBy(x => x.HeroId)
                .ToDictionary(x => x.Key,
                    x => x.Select(y => equipmentConfigurationViewModelFactory.Create(y, _equipmentViewModels))
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
}
