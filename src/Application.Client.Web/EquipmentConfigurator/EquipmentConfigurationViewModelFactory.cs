using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;
using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.ViewModels;
using Ingweland.Fog.Application.Client.Web.Factories;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator;

public class EquipmentConfigurationViewModelFactory : IEquipmentConfigurationViewModelFactory
{
    public EquipmentConfigurationViewModel Create(HeroEquipmentConfiguration srcConfiguration,
        IReadOnlyDictionary<int, EquipmentItemViewModel2> equipment)
    {
        return new EquipmentConfigurationViewModel
        {
            Id = srcConfiguration.Id,
            Hand = srcConfiguration.HandId != null
                ? equipment[srcConfiguration.HandId.Value]
                : null,
            Garment = srcConfiguration.GarmentId != null
                ? equipment[srcConfiguration.GarmentId.Value]
                : null,
            Hat = srcConfiguration.HatId != null
                ? equipment[srcConfiguration.HatId.Value]
                : null,
            Neck = srcConfiguration.NeckId != null
                ?equipment[srcConfiguration.NeckId.Value]
                : null,
            Ring = srcConfiguration.RingId != null
                ? equipment[srcConfiguration.RingId.Value]
                : null,
            IsInGame = srcConfiguration.IsInGame,
        };
    }

    
}
