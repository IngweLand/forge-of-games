using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;
using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.ViewModels;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.WebApp.Client.Components.Elements;
using Ingweland.Fog.WebApp.Client.Components.Elements.EquipmentConfigurator;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public partial class EquipmentConfiguratorPage : FogPageBase
{
    private Dictionary<string, object> _baseAnalyticsParams = null!;
    private HeroFilterRequest _filterRequest = HeroFilterRequest.Empty;
    private IReadOnlyCollection<HeroEquipmentViewModel>? _heroes;

    [Inject]
    private IAnalyticsService AnalyticsService { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }

    [Inject]
    private IEquipmentConfiguratorUiService EquipmentConfiguratorUiService { get; set; }

    [Inject]
    private IEquipmentSlotTypeIconUrlProvider EquipmentSlotTypeIconUrlProvider { get; set; }

    [Inject]
    public IFogSharingUiService FogSharingUiService { get; set; }

    [Inject]
    public IJSInteropService JsInteropService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected override bool ShouldHidePageLoadingIndicator => false;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        _baseAnalyticsParams = new Dictionary<string, object>
        {
            {AnalyticsParams.EQUIPMENT_PROFILE_ID, ProfileId},
        };

        _ = AnalyticsService.TrackEvent(AnalyticsEvents.OPEN_EQUIPMENT_CONFIGURATOR_INIT, _baseAnalyticsParams);
        
        await JsInteropService.ShowLoadingIndicatorAsync();
        await Task.Delay(30); // ensure loading indicator renders
        StateHasChanged();

        if (await EquipmentConfiguratorUiService.InitializeAsync(ProfileId))
        {
            await OnFilterChanged(HeroFilterRequest.Empty);
            await JsInteropService.HideLoadingIndicatorAsync();
            _ = AnalyticsService.TrackEvent(AnalyticsEvents.OPEN_EQUIPMENT_CONFIGURATOR_SUCCESS, _baseAnalyticsParams);
        }
        else
        {
            await JsInteropService.HideLoadingIndicatorAsync();
            _ = AnalyticsService.TrackEvent(AnalyticsEvents.OPEN_EQUIPMENT_CONFIGURATOR_ERROR, _baseAnalyticsParams);
            NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.COMMAND_CENTER_EQUIPMENT_CONFIGURATOR_DASHBOARD_PATH,
                false, true);
        }
    }

    private async Task OnFilterChanged(HeroFilterRequest request)
    {
        _filterRequest = request;
        _heroes = await EquipmentConfiguratorUiService.GetHeroes(_filterRequest);
    }

    private async Task CreateEquipmentConfiguration(HeroEquipmentViewModel heroEquipment)
    {
        var newConfig = await EquipmentConfiguratorUiService.CreateEquipmentConfigurationAsync(heroEquipment);
        await EditConfiguration(newConfig, false);
        _ = AnalyticsService.TrackEvent(AnalyticsEvents.CREATE_EQUIP_CONFIG, _baseAnalyticsParams);
    }

    private static DialogOptions GetDefaultDialogOptions(MaxWidth maxWidth = MaxWidth.ExtraExtraLarge,
        bool noHeader = true)
    {
        return new DialogOptions
        {
            MaxWidth = maxWidth,
            FullWidth = true,
            BackgroundClass = "dialog-blur-bg",
            Position = DialogPosition.TopCenter,
            CloseButton = false,
            CloseOnEscapeKey = false,
            NoHeader = noHeader,
        };
    }

    private async Task EditConfiguration(EquipmentConfigurationViewModel equipmentConfiguration,
        bool shouldTrack = true)
    {
        if (shouldTrack)
        {
            _ = AnalyticsService.TrackEvent(AnalyticsEvents.EDIT_EQUIP_CONFIG, _baseAnalyticsParams);
        }

        var options = GetDefaultDialogOptions();
        var parameters = new DialogParameters<EquipmentConfigurationDialog>
        {
            {x => x.EquipmentConfiguration, equipmentConfiguration},
        };
        _ = await DialogService.ShowAsync<EquipmentConfigurationDialog>(null, parameters, options);
    }

    private async Task DuplicateConfiguration(string equipmentConfigurationId)
    {
        _ = AnalyticsService.TrackEvent(AnalyticsEvents.DUPLICATE_EQUIP_CONFIG, _baseAnalyticsParams);
        _ = await EquipmentConfiguratorUiService.DuplicateEquipmentConfigurationAsync(equipmentConfigurationId);
    }

    private void DeleteConfiguration(EquipmentConfigurationViewModel equipmentConfiguration)
    {
        _ = AnalyticsService.TrackEvent(AnalyticsEvents.DELETE_EQUIP_CONFIG, _baseAnalyticsParams);
        EquipmentConfiguratorUiService.DeleteEquipmentConfiguration(equipmentConfiguration);
    }

    private async Task ShareProfile()
    {
        var data = FogSharingUiService.CreateSharedData(EquipmentConfiguratorUiService.Profile);
        var parameters = new DialogParameters<ShareResourceDialog>
        {
            {d => d.Data, data},
            {
                d => d.BaseUrl,
                $"{NavigationManager.BaseUri.TrimEnd('/')}{
                    FogUrlBuilder.PageRoutes.GET_SHARED_EQUIPMENT_PROFILE_TEMPLATE}"
            },
        };
        _ = await DialogService.ShowAsync<ShareResourceDialog>(null, parameters,
            GetDefaultDialogOptions(MaxWidth.Medium));
    }

    private async Task OpenSettings()
    {
        var dialog =
            await DialogService.ShowAsync<EquipmentConfiguratorSettingsDialog>(Loc[FogResource.Common_Settings],
                GetDefaultDialogOptions(MaxWidth.Large, false));
        await dialog.Result; // ensures proper page updates
    }
}
