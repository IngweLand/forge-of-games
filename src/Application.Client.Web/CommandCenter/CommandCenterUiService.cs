using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Calculators.Interfaces;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter;

public class CommandCenterUiService(
    ICommandCenterService commandCenterService,
    IPersistenceService persistenceService,
    IHeroProgressionCalculators heroProgressionCalculators,
    IHohCommandCenterProfileFactory commandCenterProfileFactory,
    IMapper mapper) : ICommandCenterUiService
{
    public event Action? StateHasChanged;
    private readonly SemaphoreSlim _initLock = new(1, 1);
    private bool _isInitialized;
    private IDictionary<string, CcProfileBasicsViewModel> _profiles = new Dictionary<string, CcProfileBasicsViewModel>();
    public CommandCenterDataDto CommandCenterData { get; private set; }
    public IReadOnlyDictionary<string, HeroDto> Heroes { get; private set; }

    public async Task<string> CreateProfileAsync(string profileName)
    {
        await EnsureInitializedAsync();

        var profile = commandCenterProfileFactory.Create(profileName);
        await persistenceService.SaveProfile(mapper.Map<BasicCommandCenterProfile>(profile));
        _profiles.Add(profile.Id, mapper.Map<CcProfileBasicsViewModel>(profile));
        NotifyStateChanged();
        return profile.Id;
    }

    public async Task<string> CreateProfileAsync(string profileName, BasicCommandCenterProfile profileDto)
    {
        await EnsureInitializedAsync();

        var clone = mapper.Map<BasicCommandCenterProfile>(profileDto);
        clone.Name = profileName;
        await persistenceService.SaveProfile(clone);
        _profiles.Add(clone.Id, mapper.Map<CcProfileBasicsViewModel>(clone));
        NotifyStateChanged();
        return clone.Id;
    }

    public async Task<bool> DeleteProfileAsync(string profileId)
    {
        await EnsureInitializedAsync();
        if (_profiles.Remove(profileId))
        {
            var success = await persistenceService.DeleteProfile(profileId);
            if (success)
            {
                CurrentProfile = null;
            }

            return success;
        }

        return false;
    }

    public async Task<IReadOnlyCollection<CcProfileBasicsViewModel>> GetProfiles()
    {
        await EnsureInitializedAsync();
        await LoadProfiles();
        return _profiles.Values.ToList();
    }

    public async Task EnsureInitializedAsync()
    {
        try
        {
            await _initLock.WaitAsync();
            if (_isInitialized)
            {
                return;
            }

            CommandCenterData = await commandCenterService.GetCommandCenterDataAsync();
            Heroes = CommandCenterData.Heroes.ToDictionary(h => h.Id);
            await LoadProfiles();
            _isInitialized = true;
        }
        finally
        {
            _initLock.Release();
        }
    }

    public CommandCenterProfile? CurrentProfile { get; set; }

    public IReadOnlyCollection<IconLabelItemViewModel> CalculateHeroProgressionCost(HeroProgressionCostRequest request)
    {
        var hero = Heroes[request.HeroId];
        return mapper.Map<IReadOnlyCollection<IconLabelItemViewModel>>(
            heroProgressionCalculators.CalculateProgressionCost(hero, request.CurrentLevel, request.TargetLevel));
    }

    private async Task LoadProfiles()
    {
        var profiles = await persistenceService.GetProfilesAsync();
        _profiles = mapper.Map<IDictionary<string, CcProfileBasicsViewModel>>(profiles);
    }

    private void NotifyStateChanged()
    {
        StateHasChanged?.Invoke();
    }
}
