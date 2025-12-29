using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public partial class EquipmentConfiguratorDashboardPage : FogPageBase
{
    private IReadOnlyCollection<EquipmentProfileBasicData> _profiles = [];

    [Inject]
    private IAnalyticsService AnalyticsService { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Inject]
    public IEquipmentProfilePersistenceService PersistenceService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        _profiles = await PersistenceService.GetProfiles();

        _ = AnalyticsService.TrackEvent(AnalyticsEvents.OPEN_EQUIPMENT_CONFIGURATOR_DASHBOARD,
            new Dictionary<string, object>());
    }

    private void NavigateTo(string path)
    {
        NavigationManager.NavigateTo(path);
    }

    private void OpenProfile(string profileId)
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.EquipmentProfile(profileId));
    }
}
