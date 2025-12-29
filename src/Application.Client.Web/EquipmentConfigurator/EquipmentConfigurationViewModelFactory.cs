using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;
using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator;

public class EquipmentConfigurationViewModelFactory(
    IUnitStatsViewModelFactory unitStatsViewModelFactory,
    IEquipmentStatsCalculators equipmentStatsCalculators)
    : IEquipmentConfigurationViewModelFactory
{
    public EquipmentConfigurationViewModel Create(HeroEquipmentConfiguration srcConfiguration,
        HeroBasicViewModel hero, IReadOnlyDictionary<UnitStatType, float> heroBaseStats,
        IReadOnlyDictionary<int, EquipmentItemViewModel2> equipment,
        IReadOnlyDictionary<EquipmentSet, EquipmentSetDefinition> setDefinitions,
        IReadOnlyDictionary<UnitStatType, string> unitStatNames)
    {
        var hand = srcConfiguration.HandId != null
            ? equipment[srcConfiguration.HandId.Value]
            : null;
        var garment = srcConfiguration.GarmentId != null
            ? equipment[srcConfiguration.GarmentId.Value]
            : null;
        var hat = srcConfiguration.HatId != null
            ? equipment[srcConfiguration.HatId.Value]
            : null;
        var neck = srcConfiguration.NeckId != null
            ? equipment[srcConfiguration.NeckId.Value]
            : null;
        var ring = srcConfiguration.RingId != null
            ? equipment[srcConfiguration.RingId.Value]
            : null;

        var boostedStats = equipmentStatsCalculators.Calculate(hand?.EquipmentItem, garment?.EquipmentItem,
            hat?.EquipmentItem, neck?.EquipmentItem, ring?.EquipmentItem, heroBaseStats, setDefinitions);

        return new EquipmentConfigurationViewModel
        {
            Id = srcConfiguration.Id,
            Hand = hand,
            Garment = garment,
            Hat = hat,
            Neck = neck,
            Ring = ring,
            IsInGame = srcConfiguration.IsInGame,
            Stats = unitStatsViewModelFactory.CreateStatsItems(boostedStats, unitStatNames),
            Hero = hero,
        };
    }

    public EquipmentConfigurationViewModel Create(string configurationId, HeroBasicViewModel hero)
    {
        return new EquipmentConfigurationViewModel
        {
            Id = configurationId,
            Hero = hero,
        };
    }
}
