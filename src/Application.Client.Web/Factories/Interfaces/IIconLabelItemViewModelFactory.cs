using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IIconLabelItemViewModelFactory
{
    IconLabelItemViewModel CreateEquipmentAttribute(StatAttribute statAttribute,
        IReadOnlyDictionary<StatAttribute, string> statAttributes);
}
