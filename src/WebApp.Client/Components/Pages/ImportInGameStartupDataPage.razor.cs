using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages;

public partial class ImportInGameStartupDataPage : FogPageBase
{
    private InGameStartupData? _inGameStartupData;
    private bool _isImporting = true;
    private bool _isLoading = true;
    private bool _shouldImportCities = true;
    private bool _shouldImportEquipment = true;
    private bool _shouldImportProfile = true;
    private bool _shouldImportResearchState = true;

    private bool _canImport =>
        _shouldImportCities | _shouldImportProfile | _shouldImportEquipment | _shouldImportResearchState;

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Inject]
    private IPersistenceService PersistenceService { get; set; }

    [Inject]
    private IInGameStartupDataService StartupDataService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        try
        {
            _inGameStartupData = await StartupDataService.GetImportedInGameDataAsync(InGameStartupDataId!);
            _isLoading = false;
        }
        catch (Exception e)
        {
            //ignore
        }
    }

    private async Task ImportData()
    {
        if (_shouldImportProfile && _inGameStartupData?.Profile != null)
        {
            _inGameStartupData.Profile.Id = Guid.NewGuid().ToString("N");
            await PersistenceService.SaveCommandCenterProfile(_inGameStartupData.Profile);
        }

        if (_shouldImportCities && _inGameStartupData?.Cities != null)
        {
            foreach (var city in _inGameStartupData.Cities)
            {
                city.Id = Guid.NewGuid().ToString("N");
                await PersistenceService.SaveCity(city);
            }
        }

        if (_shouldImportEquipment && _inGameStartupData?.Equipment is {Count: > 0})
        {
            await PersistenceService.SaveEquipment(_inGameStartupData.Equipment);
        }

        if (_shouldImportResearchState && _inGameStartupData?.ResearchState is {Count: > 0})
        {
            foreach (var kvp in _inGameStartupData.ResearchState)
            {
                var unlocked = kvp.Value.Where(x => x.State == TechnologyState.Unlocked).Select(x => x.TechnologyId).ToList();
                if(unlocked.Count == 0)
                {
                    continue;
                }
                await PersistenceService.SaveOpenTechnologies(kvp.Key, unlocked);
            }
           
        }

        _isImporting = false;
    }

    private async Task ShowCitiesStats()
    {
        if (_inGameStartupData?.Cities != null)
        {
            foreach (var city in _inGameStartupData.Cities)
            {
                city.Id = Guid.NewGuid().ToString("N");
            }

            await PersistenceService.SaveTempCities(_inGameStartupData.Cities);

            NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.CITIES_STATS_PATH);
        }
    }
}
