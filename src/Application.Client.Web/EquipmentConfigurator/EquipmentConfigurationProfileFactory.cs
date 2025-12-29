using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Relics;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator;

public class EquipmentConfigurationProfileFactory : IEquipmentConfigurationProfileFactory
{
    public EquipmentConfigurationProfile Create(IReadOnlyCollection<HeroProfileIdentifier> heroes,
        IReadOnlyCollection<RelicItem> relics, IReadOnlyCollection<EquipmentItem> equipment,
        BarracksProfile barracksProfile)
    {
        var configurations = equipment.Where(x => x.EquippedOnHero != null)
            .GroupBy(x => x.EquippedOnHero!)
            .Select(x => new HeroEquipmentConfiguration()
        {
            Id = Guid.NewGuid().ToString(),
            HeroId = x.Key,
            HandId = x.FirstOrDefault(y => y.EquipmentSlotType == EquipmentSlotType.Hand)?.Id,
            GarmentId = x.FirstOrDefault(y => y.EquipmentSlotType == EquipmentSlotType.Garment)?.Id,
            HatId = x.FirstOrDefault(y => y.EquipmentSlotType == EquipmentSlotType.Hat)?.Id,
            NeckId = x.FirstOrDefault(y => y.EquipmentSlotType == EquipmentSlotType.Neck)?.Id,
            RingId = x.FirstOrDefault(y => y.EquipmentSlotType == EquipmentSlotType.Ring)?.Id,
            IsInGame = true,
        })
            .ToList();
        
        return Create(heroes, relics, equipment, barracksProfile, configurations);
    }

    public EquipmentConfigurationProfile Create(IReadOnlyCollection<HeroProfileIdentifier> heroes,
        IReadOnlyCollection<RelicItem> relics,
        IReadOnlyCollection<EquipmentItem> equipment, BarracksProfile barracksProfile,
        IList<HeroEquipmentConfiguration> configurations)
    {
        return new EquipmentConfigurationProfile
        {
            Id = Guid.NewGuid().ToString("N"),
            Name = "Main",
            Heroes = heroes,
            Relics = relics,
            Equipment = equipment,
            BarracksProfile = barracksProfile,
            Configurations = configurations,
        };
    }
}
