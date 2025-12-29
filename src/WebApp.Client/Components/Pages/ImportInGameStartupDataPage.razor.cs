using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages;

public partial class ImportInGameStartupDataPage : FogPageBase
{
    private EquipmentProfileImportMethod _equipmentProfileImportMethod = EquipmentProfileImportMethod.New;
    private string? _equipmentProfileName;
    private IReadOnlyCollection<EquipmentProfileBasicData> _equipmentProfiles = [];
    private InGameStartupData? _inGameStartupData;
    private bool _isImporting = true;
    private bool _isLoading = true;
    private EquipmentProfileBasicData? _selectedEquipmentProfile;
    private bool _shouldImportCities;
    private bool _shouldImportEquipment;
    private bool _shouldImportEquipmentProfile;
    private bool _shouldImportProfile;
    private bool _shouldImportResearchState;

    private bool _canImport => _shouldImportCities | _shouldImportProfile | _shouldImportEquipment |
        _shouldImportResearchState | _shouldImportEquipmentProfile;

    [Inject]
    private IEquipmentProfilePersistenceService EquipmentPersistenceService { get; set; }

    [Inject]
    private ILogger<ImportInGameStartupDataPage> Logger { get; set; }

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
            _equipmentProfiles = await EquipmentPersistenceService.GetProfiles();
            _selectedEquipmentProfile = _equipmentProfiles.FirstOrDefault();
            _isLoading = false;
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error initializing.");
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
                var unlocked = kvp.Value.Where(x => x.State == TechnologyState.Unlocked).Select(x => x.TechnologyId)
                    .ToList();
                if (unlocked.Count == 0)
                {
                    continue;
                }

                await PersistenceService.SaveOpenTechnologies(kvp.Key, unlocked);
            }
        }

        if (_shouldImportEquipmentProfile && _inGameStartupData is {Equipment: {Count: > 0}, Profile: not null})
        {
            string? profileId = null;
            if (_equipmentProfileImportMethod == EquipmentProfileImportMethod.Sync)
            {
                profileId = _selectedEquipmentProfile?.Id;
            }

            await EquipmentPersistenceService.UpsertProfileAsync(profileId, _equipmentProfileName,
                _inGameStartupData.Profile.Heroes.AsReadOnly(),
                _inGameStartupData.Relics ?? [], _inGameStartupData.Equipment,
                _inGameStartupData.Profile.BarracksProfile);
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

    private enum EquipmentProfileImportMethod
    {
        New,
        Sync,
    }
}
