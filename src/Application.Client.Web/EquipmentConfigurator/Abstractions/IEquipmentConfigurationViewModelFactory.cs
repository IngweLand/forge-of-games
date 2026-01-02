using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;

public interface IEquipmentConfigurationViewModelFactory
{
    EquipmentConfigurationViewModel Create(HeroEquipmentConfiguration srcConfiguration,
        IReadOnlyDictionary<int, EquipmentItemViewModel2> equipment);
}
