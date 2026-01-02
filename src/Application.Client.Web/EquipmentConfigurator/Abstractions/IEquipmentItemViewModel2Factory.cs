using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.ViewModels;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;

public interface IEquipmentItemViewModel2Factory
{
    EquipmentItemViewModel2 CreateItem(EquipmentItem equipmentItem,
        IReadOnlyDictionary<string, string> setNames,
        IReadOnlyDictionary<StatAttribute, string> statAttributesNames);
}
