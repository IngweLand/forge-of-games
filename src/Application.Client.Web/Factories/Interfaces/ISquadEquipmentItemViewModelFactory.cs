using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface ISquadEquipmentItemViewModelFactory
{
    SquadEquipmentItemViewModel Create(SquadEquipmentItem src,
        IReadOnlyDictionary<StatAttribute, string> statAttributes,
        IReadOnlyDictionary<EquipmentSet, string> sets);
}
