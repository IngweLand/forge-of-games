using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;

public interface ICcEquipmentUiService
{
    Task<IReadOnlyCollection<EquipmentItemViewModel>> GetEquipmentAsync();
}