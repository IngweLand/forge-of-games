using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.ViewModels;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Relics;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;

public interface 
    IEquipmentConfiguratorUiService
{
    Task<EquipmentConfigurationProfile?> InitializeAsync();
    Task<IReadOnlyCollection<HeroEquipmentViewModel>> GetHeroes(HeroFilterRequest request);
}
