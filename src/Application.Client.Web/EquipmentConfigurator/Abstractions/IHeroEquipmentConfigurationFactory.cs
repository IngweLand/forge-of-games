using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;

public interface IHeroEquipmentConfigurationFactory
{
    HeroEquipmentConfiguration Create(IEnumerable<EquipmentItem> items, bool isInGameConfiguration);
    HeroEquipmentConfiguration Create(string heroId);
    HeroEquipmentConfiguration Duplicate(HeroEquipmentConfiguration src);
}
