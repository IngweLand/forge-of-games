using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Caching.Interfaces;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Migrations.CommandCenter.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter;

public class CcProfileUiService(
    ICommandCenterUiService commandCenterUiService,
    IHohCommandCenterTeamFactory commandCenterTeamFactory,
    IHohCommandCenterProfileFactory commandCenterProfileFactory,
    IPersistenceService persistenceService,
    IHohHeroProfileViewModelFactory heroProfileViewModelFactory,
    IHohHeroProfileFactory heroProfileFactory,
    IHeroProfileIdentifierFactory heroProfileIdentifierFactory,
    ICcProfileViewModelFactory profileViewModelFactory,
    ICcProfileTeamViewModelFactory profileTeamViewModelFactory,
    IBarracksViewModelFactory barracksViewModelFactory,
    IHohCoreDataCache coreDataCache,
    ICcMigrationManager migrationManager,
    IMapper mapper) : ICcProfileUiService
{
    private IReadOnlyDictionary<BuildingGroup, CcBarracksViewModel>? _barracksViewModels;
    private CommandCenterProfile? _currentProfile;

    private IDictionary<string, HeroProfileViewModel> _heroProfileViewModels =
        new Dictionary<string, HeroProfileViewModel>();

    public event Action? StateHasChanged;

    public BasicCommandCenterProfile GetProfileDto()
    {
        EnsureInitialized();
        return mapper.Map<BasicCommandCenterProfile>(_currentProfile);
    }

    public async Task<bool> UpdateProfileSettingsAsync(CcProfileSettings profileSettings)
    {
        EnsureInitialized();

        if (string.IsNullOrWhiteSpace(profileSettings.Name))
        {
            return false;
        }

        if (_currentProfile!.Name == profileSettings.Name)
        {
            return false;
        }

        _currentProfile.Name = profileSettings.Name;
        await SaveCurrentProfile();
        NotifyStateChanged();
        return true;
    }

    public IReadOnlyCollection<CcBarracksViewModel> GetBarracks()
    {
        EnsureInitialized();

        _barracksViewModels ??=
            barracksViewModelFactory.Create(coreDataCache.GetAllBarracks().SelectMany(x => x.Value).ToList());

        foreach (var barracksProfileLevel in _currentProfile!.BarracksProfile.Levels)
        {
            _barracksViewModels[barracksProfileLevel.Key].SelectedLevel = barracksProfileLevel.Value;
        }

        return _barracksViewModels.Values.ToList();
    }

    public async Task UpdateBarracksAsync(CcBarracksViewModel barracks)
    {
        EnsureInitialized();

        _currentProfile!.BarracksProfile.Levels[barracks.Group] = barracks.SelectedLevel;
        await SaveCurrentProfile();
        await UpdateHeroProfilesForBarracksChange(barracks.Group, barracks.SelectedLevel);
        NotifyStateChanged();
    }

    public CcProfileSettings GetSettings()
    {
        EnsureInitialized();

        return mapper.Map<CcProfileSettings>(_currentProfile);
    }

    public async Task DeleteTeamAsync(string teamId)
    {
        EnsureInitialized();

        _currentProfile!.Teams.Remove(teamId);
        await SaveCurrentProfile();
        NotifyStateChanged();
    }

    public IReadOnlyCollection<HeroBasicViewModel> GetAddableHeroesForProfile()
    {
        EnsureInitialized();

        return coreDataCache.GetAllHeroes().Values
            .Where(h => !_currentProfile!.Heroes.ContainsKey(h.Id))
            .Select(mapper.Map<HeroBasicViewModel>)
            .OrderBy(h => h.Name)
            .ToList();
    }

    public IReadOnlyCollection<HeroBasicViewModel> GetAddableHeroesForTeam(string teamId)
    {
        EnsureInitialized();

        var heroIds = _currentProfile!.Heroes.Values
            .Where(h => !_currentProfile.Teams[teamId].HeroIds.Contains(h.Identifier.HeroId))
            .Select(p => p.Identifier.HeroId).ToHashSet();
        return coreDataCache.GetAllHeroes().Values
            .Where(h => heroIds.Contains(h.Id))
            .Select(mapper.Map<HeroBasicViewModel>)
            .OrderBy(h => h.Name)
            .ToList();
    }

    public CcProfileTeamViewModel? GetTeam(string teamId)
    {
        EnsureInitialized();

        if (!_currentProfile!.Teams.TryGetValue(teamId, out var team))
        {
            return null;
        }

        return profileTeamViewModelFactory.Create(team, _heroProfileViewModels.AsReadOnly());
    }

    public async Task RemoveHeroFromTeamAsync(string teamId, string heroProfileId)
    {
        EnsureInitialized();

        if (_currentProfile!.Teams[teamId].HeroIds.Remove(heroProfileId))
        {
            await SaveCurrentProfile();
            NotifyStateChanged();
        }
    }

    public async Task<string?> CreateTeamAsync(string teamName)
    {
        EnsureInitialized();

        var team = commandCenterTeamFactory.Create(teamName);
        _currentProfile!.Teams.Add(team.Id, team);
        await SaveCurrentProfile();
        NotifyStateChanged();
        return team.Id;
    }

    public async Task AddHeroToTeamAsync(string teamId, string heroId)
    {
        EnsureInitialized();

        var heroProfile = _currentProfile!.Heroes.Values.FirstOrDefault(hp => hp.Identifier.HeroId == heroId);
        if (heroProfile == null)
        {
            return;
        }

        if (!_currentProfile.Teams.TryGetValue(teamId, out var team))
        {
            return;
        }

        if (team.HeroIds.Contains(heroProfile.Identifier.HeroId) ||
            team.HeroIds.Count >= HohConstants.MAX_TEAM_MEMBERS)
        {
            return;
        }

        team.HeroIds.Add(heroProfile.Identifier.HeroId);
        await SaveCurrentProfile();

        NotifyStateChanged();
    }

    public async Task UpdateHeroProfileAsync(HeroProfileIdentifier identifier)
    {
        EnsureInitialized();

        if (!_currentProfile!.Heroes.ContainsKey(identifier.HeroId))
        {
            throw new InvalidOperationException("Hero profile not found.");
        }

        var hero = await coreDataCache.GetHeroAsync(identifier.HeroId);
        if (hero == null)
        {
            throw new InvalidOperationException("Hero not found.");
        }

        var barracks = await coreDataCache.GetBarracks(hero.Unit.Type);
        var heroBarracks = barracks.First(b => b.Level == identifier.BarracksLevel);
        _ = await UpdateHeroProfile(identifier, hero, heroBarracks);

        await SaveCurrentProfile();
    }

    public async Task<CcProfileViewModel?> InitializedAsync(string profileId)
    {
        if (_currentProfile != null && _currentProfile.Id == profileId)
        {
            return profileViewModelFactory.Create(_currentProfile, _heroProfileViewModels.AsReadOnly());
        }

        await commandCenterUiService.EnsureInitializedAsync();

        _currentProfile = null;
        _heroProfileViewModels.Clear();

        var profileDto = await persistenceService.LoadProfile(profileId);

        if (profileDto == null)
        {
            return null;
        }

        profileDto = migrationManager.Migrate(profileDto);

        var profile = commandCenterProfileFactory.Create(profileDto, coreDataCache.GetAllHeroes(),
            coreDataCache.GetAllBarracks().SelectMany(x => x.Value).ToList());

        List<HeroProfileViewModel> profileViewModels = [];
        foreach (var heroProfile in profile.Heroes.Values)
        {
            profileViewModels.Add(await CreateHeroProfileViewModel(heroProfile));
        }

        _currentProfile = profile;
        _heroProfileViewModels = profileViewModels.ToDictionary(hp => hp.Identifier.HeroId);

        return profileViewModelFactory.Create(_currentProfile, _heroProfileViewModels.AsReadOnly());
    }

    public CcProfileViewModel GetCurrentProfile()
    {
        EnsureInitialized();
        return profileViewModelFactory.Create(_currentProfile!, _heroProfileViewModels.AsReadOnly());
    }

    public async Task<IReadOnlyCollection<HeroProfileViewModel>> GetProfileHeroesAsync()
    {
        EnsureInitialized();

        return _heroProfileViewModels.Values.ToList().AsReadOnly();
    }

    public async Task AddHeroAsync(string heroId)
    {
        EnsureInitialized();

        if (_currentProfile!.Heroes.ContainsKey(heroId))
        {
            return;
        }

        var newHeroProfileDto = heroProfileIdentifierFactory.Create(heroId);
        var hero = await coreDataCache.GetHeroAsync(heroId);
        var group = hero!.Unit.Type.ToBuildingGroup();
        var barracks = await coreDataCache.GetBarracks(hero.Unit.Type);
        var heroBarracks =
            barracks.First(b => b.Level == _currentProfile.BarracksProfile.Levels[group]);
        var newHeroProfile = heroProfileFactory.Create(newHeroProfileDto, hero, heroBarracks);
        var vm = await CreateHeroProfileViewModel(newHeroProfile);
        _currentProfile.Heroes.Add(newHeroProfile.Identifier.HeroId, newHeroProfile);
        _heroProfileViewModels.Add(newHeroProfile.Identifier.HeroId, vm);
        await SaveCurrentProfile();

        NotifyStateChanged();
    }

    public async Task RemoveHeroFromProfileAsync(string heroId)
    {
        EnsureInitialized();

        if (!_currentProfile!.Heroes.Remove(heroId))
        {
            return;
        }

        foreach (var team in _currentProfile.Teams.Values)
        {
            team.HeroIds.Remove(heroId);
        }

        _heroProfileViewModels.Remove(heroId);

        await SaveCurrentProfile();

        NotifyStateChanged();
    }

    public async Task<HeroProfileIdentifier> GetHeroProfileIdentifierAsync(string heroId)
    {
        EnsureInitialized();
        var hero = await coreDataCache.GetHeroAsync(heroId);
        var barracksLevel = _currentProfile!.BarracksProfile.Levels[hero!.Unit.Type.ToBuildingGroup()];
        return _currentProfile!.Heroes.Values.First(hp => hp.Identifier.HeroId == heroId).Identifier with
        {
            BarracksLevel = barracksLevel,
        };
    }

    private void EnsureInitialized()
    {
        if (_currentProfile == null)
        {
            throw new InvalidOperationException("Current profile not initialized.");
        }
    }

    private async Task UpdateHeroProfilesForBarracksChange(BuildingGroup barracks, int level)
    {
        foreach (var heroProfile in _currentProfile!.Heroes.Values)
        {
            var hero = await coreDataCache.GetHeroAsync(heroProfile.Identifier.HeroId);
            var group = hero!.Unit.Type.ToBuildingGroup();
            if (group != barracks)
            {
                continue;
            }

            var heroBarracks = (await coreDataCache.GetBarracks(hero.Unit.Type)).First(b => b.Level == level);
            _ = UpdateHeroProfile(heroProfile.Identifier, hero, heroBarracks);
        }
    }

    private async Task<HeroProfileViewModel> UpdateHeroProfile(HeroProfileIdentifier identifier, HeroDto hero,
        BuildingDto barracks)
    {
        var newHeroProfile = heroProfileFactory.Create(identifier, hero, barracks);
        var newViewModel = await CreateHeroProfileViewModel(newHeroProfile);
        _currentProfile!.Heroes[identifier.HeroId] = newHeroProfile;
        _heroProfileViewModels[identifier.HeroId] = newViewModel;
        return newViewModel;
    }

    private async Task<HeroProfileViewModel> CreateHeroProfileViewModel(HeroProfile heroProfile)
    {
        var hero = await coreDataCache.GetHeroAsync(heroProfile.Identifier.HeroId);
        return heroProfileViewModelFactory.Create(heroProfile, hero!, []);
    }

    private void NotifyStateChanged()
    {
        StateHasChanged?.Invoke();
    }

    private ValueTask SaveCurrentProfile()
    {
        return persistenceService.SaveCommandCenterProfile(mapper.Map<BasicCommandCenterProfile>(_currentProfile));
    }
}
