using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Ingweland.Fog.WebApp.Client.Components.Pages;

public partial class ImportInGameStartupDataPage : ComponentBase
{
    private InGameStartupData? _inGameStartupData;
    private bool _isImporting = true;
    private bool _isLoading = true;
    private bool _shouldImportCities = true;
    private bool _shouldImportProfile = true;

    private bool _canImport => _shouldImportCities | _shouldImportProfile;

    [Inject]
    protected IStringLocalizer<FogResource> Loc { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Inject]
    private IPersistenceService PersistenceService { get; set; }

    [Inject]
    private IInGameStartupDataService StartupDataService { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        try
        {
            _inGameStartupData = await StartupDataService.GetImportedInGameDataAsync(InGameStartupDataId!);
        }
        catch (Exception e)
        {
            // ignored
        }

        _isLoading = false;
        StateHasChanged();
    }

    protected override void OnInitialized()
    {
        if (string.IsNullOrWhiteSpace(InGameStartupDataId))
        {
            NavigationManager.NavigateTo("/");
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
