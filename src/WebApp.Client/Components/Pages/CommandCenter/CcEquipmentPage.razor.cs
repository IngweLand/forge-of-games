using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public partial class CcEquipmentPage : CommandCenterPageBase
{
    private IReadOnlyCollection<EquipmentItemViewModel>? _equipment;

    [Inject]
    private ICcEquipmentUiService EquipmentUiService { get; set; }

    protected override async Task HandleOnInitializedAsync()
    {
        await base.HandleOnInitializedAsync();

        _equipment = await EquipmentUiService.GetEquipmentAsync();
    }
}