using Ingweland.Fog.Application.Client.Web.Constants;
using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator;
using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Ingweland.Fog.WebApp.Client.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages;

public partial class EquipmentConfiguratorPage : FogPageBase
{
    private HeroFilterRequest _filterRequest = HeroFilterRequest.Empty;
    private IReadOnlyCollection<HeroEquipmentViewModel>? _heroes;
    private string? _searchString = string.Empty;

    [Inject]
    private IEquipmentConfiguratorUiService EquipmentConfiguratorUiService { get; set; }
    
    [Inject]
    private IEquipmentSlotTypeIconUrlProvider EquipmentSlotTypeIconUrlProvider { get; set; }
    
    [Inject]
    private IDialogService DialogService { get; set; }

    protected override bool ShouldHidePageLoadingIndicator => false;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (!OperatingSystem.IsBrowser())
        {
            return;
        }
        await JsInteropService.ShowLoadingIndicatorAsync();
        StateHasChanged();
        
        await EquipmentConfiguratorUiService.InitializeAsync();
        await OnFilterChanged(HeroFilterRequest.Empty);

        await JsInteropService.HideLoadingIndicatorAsync();
    }

    private async Task OnFilterChanged(HeroFilterRequest request)
    {
        _filterRequest = request;
        _heroes = await EquipmentConfiguratorUiService.GetHeroes(_filterRequest);
    }
    
    private async Task OnDeleteClicked(string id)
    {
        var result = await DialogService.ShowMessageBox(
            null,
            Loc[FogResource.Common_DeleteConfirmation, id],
            Loc[FogResource.Common_Delete], cancelText: Loc[FogResource.Common_Cancel]);
        if (result != null && result.Value)
        {
            await OnDelete.InvokeAsync(id);
        }
    }
}
