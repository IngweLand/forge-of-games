using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;

public interface IHeroEquipmentConfigurationViewModelFactory
{
    HeroEquipmentConfigurationViewModel Create(HeroEquipmentConfiguration srcConfiguration,
        IReadOnlyDictionary<int, EquipmentItemViewModel> equipment, IReadOnlyDictionary<StatAttribute, string> statAttributes,IReadOnlyDictionary<EquipmentSet, string> sets);
}
