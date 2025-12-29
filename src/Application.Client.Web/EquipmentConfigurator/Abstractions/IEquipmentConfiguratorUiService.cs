using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.ViewModels;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;

public interface IEquipmentConfiguratorUiService
{
    IReadOnlyDictionary<EquipmentSet, string> EquipmentSets { get; }
    EquipmentProfile Profile { get; }
    IReadOnlyDictionary<StatAttribute, string> StatAttributes { get; }
    Task<bool> InitializeAsync(string profileId);
    Task<IReadOnlyCollection<HeroEquipmentViewModel>> GetHeroes(HeroFilterRequest request);

    Task<EquipmentConfigurationViewModel> CreateEquipmentConfigurationAsync(HeroEquipmentViewModel heroEquipment);

    IReadOnlyCollection<EquipmentItemViewModel2> GetEquipment(EquipmentSlotType slot, EquipmentFilterRequest request);

    Task<EquipmentConfigurationViewModel> DuplicateEquipmentConfigurationAsync(string equipmentConfigurationId);

    void DeleteEquipmentConfiguration(EquipmentConfigurationViewModel equipmentConfiguration);

    Task<IReadOnlyCollection<EquipmentSlotWithSetViewModel>> GetEquipmentSlotTypesAsync(
        EquipmentConfigurationViewModel equipmentConfiguration);

    Task SetEquipmentAsync(EquipmentConfigurationViewModel equipmentConfiguration,
        EquipmentSlotType slot, EquipmentItemViewModel2? equipment);

    Task UpdateProfileNameAsync(string profileName);
}
