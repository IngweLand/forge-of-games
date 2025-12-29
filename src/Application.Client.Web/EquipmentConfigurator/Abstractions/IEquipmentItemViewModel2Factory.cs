using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;

public interface IEquipmentItemViewModel2Factory
{
    EquipmentItemViewModel2 CreateItem(EquipmentItem equipmentItem, IEnumerable<HeroBasicViewModel> heroes,
        IReadOnlyDictionary<string, string> setNames, IReadOnlyDictionary<StatAttribute, string> statAttributesNames);
}
