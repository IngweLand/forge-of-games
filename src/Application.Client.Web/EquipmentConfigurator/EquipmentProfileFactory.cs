using Ingweland.Fog.Application.Client.Core.Localization;
using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Relics;
using Microsoft.Extensions.Localization;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator;

public class EquipmentProfileFactory(
    IHeroEquipmentConfigurationFactory equipmentConfigurationFactory,
    IStringLocalizer<FogResource> loc)
    : IEquipmentProfileFactory
{
    public EquipmentProfile Create(string? profileName, IReadOnlyCollection<HeroProfileIdentifier> heroes,
        IReadOnlyCollection<RelicItem> relics, IReadOnlyCollection<EquipmentItem> equipment,
        BarracksProfile barracksProfile)
    {
        return Create(profileName, heroes, relics, equipment, barracksProfile, CreateConfigurations(equipment));
    }

    public IList<HeroEquipmentConfiguration> CreateConfigurations(IReadOnlyCollection<EquipmentItem> equipment)
    {
        return equipment
            .Where(x => x.EquippedOnHero != null)
            .GroupBy(x => x.EquippedOnHero!)
            .Select(x => equipmentConfigurationFactory.Create(x, true))
            .ToList();
    }

    public EquipmentProfile Create(string? profileName, IReadOnlyCollection<HeroProfileIdentifier> heroes,
        IReadOnlyCollection<RelicItem> relics,
        IReadOnlyCollection<EquipmentItem> equipment, BarracksProfile barracksProfile,
        IList<HeroEquipmentConfiguration> configurations)
    {
        return new EquipmentProfile
        {
            Id = Guid.NewGuid().ToString("N"),
            Name = profileName ?? loc[FogResource.Common_Main],
            Heroes = heroes,
            Relics = relics,
            Equipment = equipment,
            BarracksProfile = barracksProfile,
            Configurations = configurations,
            UpdatedAt = DateTime.Now.ToLocalTime(),
        };
    }
}
