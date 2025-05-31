using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh;

public class EquipmentUiProfile :Profile
{
    public EquipmentUiProfile()
    {
        CreateMap<EquipmentItem, EquipmentItemViewModel>().ConvertUsing<EquipmentItemToViewModelConverter>();
    }
}