using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public interface IEquipmentItemAttributeViewModelFactory
{
    Dictionary<StatAttribute, NumericValueType> StatToNumericValueTypeMap { get; }
    EquipmentItemAttributeViewModel CreateAttribute(EquipmentAttribute attribute);

    EquipmentItemSubAttributeViewModel? CreateSubAttribute(IEnumerable<EquipmentAttribute> attributes,
        StatAttribute targetAttribute);

    IconLabelsItemViewModel CreateAttribute(EquipmentAttribute attribute,
        IReadOnlyDictionary<StatAttribute, string> statAttributeNames);

    EquipmentItemSubAttributeViewModel2 CreateSubAttribute(EquipmentAttribute attribute,
        IReadOnlyDictionary<StatAttribute, string> statAttributeNames);
}
