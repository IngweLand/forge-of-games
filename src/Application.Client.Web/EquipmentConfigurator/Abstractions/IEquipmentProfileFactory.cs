using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Relics;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;

public interface IEquipmentProfileFactory
{
    EquipmentProfile Create(string? profileName, IReadOnlyCollection<HeroProfileIdentifier> heroes,
        IReadOnlyCollection<RelicItem> relics, IReadOnlyCollection<EquipmentItem> equipment,
        BarracksProfile barracksProfile);

    EquipmentProfile Create(string? profileName, IReadOnlyCollection<HeroProfileIdentifier> heroes,
        IReadOnlyCollection<RelicItem> relics, IReadOnlyCollection<EquipmentItem> equipment,
        BarracksProfile barracksProfile, IList<HeroEquipmentConfiguration> configurations);

    IList<HeroEquipmentConfiguration> CreateConfigurations(IReadOnlyCollection<EquipmentItem> equipment);
}
