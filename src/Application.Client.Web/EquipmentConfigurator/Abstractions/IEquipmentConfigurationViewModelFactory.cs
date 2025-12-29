using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;

public interface IEquipmentConfigurationViewModelFactory
{
    EquipmentConfigurationViewModel Create(HeroEquipmentConfiguration srcConfiguration,
        HeroBasicViewModel hero, IReadOnlyDictionary<UnitStatType, float> heroBaseStats,
        IReadOnlyDictionary<int, EquipmentItemViewModel2> equipment,
        IReadOnlyDictionary<EquipmentSet, EquipmentSetDefinition> setDefinitions,
        IReadOnlyDictionary<UnitStatType, string> unitStatNames);

    EquipmentConfigurationViewModel Create(string configurationId, HeroBasicViewModel hero);
}
