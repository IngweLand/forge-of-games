using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Ingweland.Fog.WebApp.Client.Components.Pages;

public partial class ImportInGameStartupDataPage : FogPageBase
{
    private InGameStartupData? _inGameStartupData;
    private bool _isImporting = true;
    private bool _isLoading = true;
    private bool _shouldImportCities = true;
    private bool _shouldImportProfile = true;

    private bool _canImport => _shouldImportCities | _shouldImportProfile;

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
            await PersistenceService.SaveProfile(_inGameStartupData.Profile);
        }

        if (_shouldImportCities && _inGameStartupData?.Cities != null)
        {
            foreach (var city in _inGameStartupData.Cities)
            {
                city.Id = Guid.NewGuid().ToString("N");
                await PersistenceService.SaveCity(city);
            }
        }

        _isImporting = false;
    }
}
