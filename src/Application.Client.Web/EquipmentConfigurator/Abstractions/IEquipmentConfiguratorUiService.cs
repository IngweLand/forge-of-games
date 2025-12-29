using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Relics;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;

public interface 
    IEquipmentConfiguratorUiService
{
    Task UpsertProfileAsync(IReadOnlyCollection<HeroProfileIdentifier> heroes,
        IReadOnlyCollection<RelicItem> relics, IReadOnlyCollection<EquipmentItem> equipment,
        BarracksProfile barracksProfile);

    Task<EquipmentConfigurationProfile?> InitializeAsync();
    Task<IReadOnlyCollection<HeroEquipmentViewModel>> GetHeroes(HeroFilterRequest request);
}
