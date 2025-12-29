using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator;

public class HeroEquipmentConfigurationFactory : IHeroEquipmentConfigurationFactory
{
    public HeroEquipmentConfiguration Create(IEnumerable<EquipmentItem> items, bool isInGameConfiguration)
    {
        var equipmentList = items.ToList();
        return new HeroEquipmentConfiguration
        {
            Id = Guid.NewGuid().ToString(),
            HeroId = equipmentList.First().EquippedOnHero!,
            HandId = equipmentList.FirstOrDefault(y => y.EquipmentSlotType == EquipmentSlotType.Hand)?.Id,
            GarmentId = equipmentList.FirstOrDefault(y => y.EquipmentSlotType == EquipmentSlotType.Garment)?.Id,
            HatId = equipmentList.FirstOrDefault(y => y.EquipmentSlotType == EquipmentSlotType.Hat)?.Id,
            NeckId = equipmentList.FirstOrDefault(y => y.EquipmentSlotType == EquipmentSlotType.Neck)?.Id,
            RingId = equipmentList.FirstOrDefault(y => y.EquipmentSlotType == EquipmentSlotType.Ring)?.Id,
            IsInGame = isInGameConfiguration,
        };
    }

    public HeroEquipmentConfiguration Create(string heroId)
    {
        return new HeroEquipmentConfiguration
        {
            Id = Guid.NewGuid().ToString(),
            HeroId = heroId,
        };
    }

    public HeroEquipmentConfiguration Duplicate(HeroEquipmentConfiguration src)
    {
        return new HeroEquipmentConfiguration
        {
            Id = Guid.NewGuid().ToString(),
            HeroId = src.HeroId,
            HandId = src.HandId,
            GarmentId = src.GarmentId,
            HatId = src.HatId,
            NeckId = src.NeckId,
            RingId = src.RingId,
        };
    }
}
