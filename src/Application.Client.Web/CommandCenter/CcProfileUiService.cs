using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
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
    IHohHeroProfileDtoFactory heroProfileDtoFactory,
    ICcProfileViewModelFactory profileViewModelFactory,
    ICcProfileTeamViewModelFactory profileTeamViewModelFactory,
    IBarracksViewModelFactory barracksViewModelFactory,
    IMapper mapper) : ICcProfileUiService
{
    public event Action? StateHasChanged;
    private IReadOnlyDictionary<BuildingGroup, CcBarracksViewModel>? _barracksViewModels;

    private IDictionary<string, HeroProfileViewModel> _heroProfileViewModels =
        new Dictionary<string, HeroProfileViewModel>();

    public async Task<CcProfileViewModel?> GetProfileAsync(string profileId)
    {
        if (!await EnsureInitializedAsync(profileId))
        {
            return null;
        }

        return profileViewModelFactory.Create(commandCenterUiService.CurrentProfile!, _heroProfileViewModels.AsReadOnly());
    }

    public async Task<BasicCommandCenterProfile?> GetProfileDtoAsync(string profileId)
    {
        if (!await EnsureInitializedAsync(profileId))
        {
            return null;
        }

        return mapper.Map<BasicCommandCenterProfile>(commandCenterUiService.CurrentProfile);
    }
    
    public async Task<bool> UpdateProfileSettingsAsync(string profileId, CcProfileSettings profileSettings)
    {
        if (string.IsNullOrWhiteSpace(profileSettings.Name))
        {
            return false;
        }
        
        if (!await EnsureInitializedAsync(profileId))
        {
            return false;
        }

        if (commandCenterUiService.CurrentProfile!.Name == profileSettings.Name)
        {
            return false;
        }

        commandCenterUiService.CurrentProfile.Name = profileSettings.Name;
        await SaveCurrentProfile();
        NotifyStateChanged();
        return true;
    }

    public async Task<IReadOnlyCollection<CcBarracksViewModel>> GetBarracks(string profileId)
    {
        if (!await EnsureInitializedAsync(profileId))
        {
            return [];
        }
        
        _barracksViewModels ??= barracksViewModelFactory.Create(commandCenterUiService.CommandCenterData.Barracks);

        foreach (var barracksProfileLevel in commandCenterUiService.CurrentProfile!.BarracksProfile.Levels)
        {
            _barracksViewModels[barracksProfileLevel.Key].SelectedLevel = barracksProfileLevel.Value;
        }

        return _barracksViewModels.Values.ToList();
    }
    
    public async Task UpdateBarracks(string profileId, CcBarracksViewModel barracks)
    {
        if (!await EnsureInitializedAsync(profileId))
        {
            return ;
        }

        commandCenterUiService.CurrentProfile!.BarracksProfile.Levels[barracks.Group] = barracks.SelectedLevel;
        await SaveCurrentProfile();
        UpdateHeroProfilesForBarracksChange(barracks.Group, barracks.SelectedLevel);
        NotifyStateChanged();
    }
    
    private void UpdateHeroProfilesForBarracksChange(BuildingGroup barracks, int level)
    {
        var updatable = new HashSet<string>();
        foreach (var heroProfile in commandCenterUiService.CurrentProfile!.Heroes.Values)
        {
            var hero = commandCenterUiService.Heroes[heroProfile.HeroId];
            var group = hero.Unit.Type.ToBuildingGroup();
            if (group != barracks)
            {
                continue;
            }

            updatable.Add(heroProfile.Id);
        }

        foreach (var id in updatable)
        {
            var heroProfile = commandCenterUiService.CurrentProfile!.Heroes[id];
            var hero = commandCenterUiService.Heroes[heroProfile.HeroId];
            var heroBarracks = commandCenterUiService.CommandCenterData.Barracks.First(b =>
                b.Group == barracks && b.Level == level);
            _ = UpdateHeroProfile(heroProfile, hero, heroBarracks);
        }
    }

    public async Task<IReadOnlyCollection<HeroProfileViewModel>> GetProfileHeroesAsync(string profileId)
    {
        if (!await EnsureInitializedAsync(profileId))
        {
            return [];
        }

        return _heroProfileViewModels.Values.ToList().AsReadOnly();
    }

    public async Task AddHeroAsync(string profileId, string heroId)
    {
        if (!await EnsureInitializedAsync(profileId))
        {
            return;
        }

        if (commandCenterUiService.CurrentProfile!.Heroes.Values.Any(hp => hp.HeroId == heroId))
        {
            return;
        }

        var newHeroProfileDto = heroProfileDtoFactory.Create(heroId);
        var hero = commandCenterUiService.Heroes[heroId];
        var group = hero.Unit.Type.ToBuildingGroup();
        var heroBarracks = commandCenterUiService.CommandCenterData.Barracks.First(b =>
            b.Group == group && b.Level == commandCenterUiService.CurrentProfile.BarracksProfile.Levels[group]);
        var newHeroProfile = heroProfileFactory.Create(newHeroProfileDto, hero, heroBarracks);
        var vm = CreateHeroProfileViewModel(newHeroProfile);
        commandCenterUiService.CurrentProfile.Heroes.Add(newHeroProfile.Id, newHeroProfile);
        _heroProfileViewModels.Add(newHeroProfile.Id, vm);
        await SaveCurrentProfile();

        NotifyStateChanged();
    }

    public async Task RemoveHeroFromProfileAsync(string profileId, string heroProfileId)
    {
        if (!await EnsureInitializedAsync(profileId))
        {
            return;
        }

        if (!commandCenterUiService.CurrentProfile!.Heroes.ContainsKey(heroProfileId))
        {
            return;
        }

        commandCenterUiService.CurrentProfile.Heroes.Remove(heroProfileId);
        foreach (var team in commandCenterUiService.CurrentProfile.Teams.Values)
        {
            team.HeroProfileIds.Remove(heroProfileId);
        }

        _heroProfileViewModels.Remove(heroProfileId);

        await SaveCurrentProfile();

        NotifyStateChanged();
    }
    
    public async Task<CcProfileSettings?> GetSettingsAsync(string profileId)
    {
        if (!await EnsureInitializedAsync(profileId))
        {
            return null;
        }

        return mapper.Map<CcProfileSettings>(commandCenterUiService.CurrentProfile);
    }

    public async Task DeleteTeamAsync(string profileId, string teamId)
    {
        if (!await EnsureInitializedAsync(profileId))
        {
            return;
        }

        commandCenterUiService.CurrentProfile!.Teams.Remove(teamId);
        await SaveCurrentProfile();
        NotifyStateChanged();
    }

    public async Task<IReadOnlyCollection<HeroBasicViewModel>> GetAddableHeroesForProfileAsync(string profileId)
    {
        if (!await EnsureInitializedAsync(profileId))
        {
            return [];
        }

        return commandCenterUiService.Heroes.Values
            .Where(h => commandCenterUiService.CurrentProfile!.Heroes.Values.All(hp => hp.HeroId != h.Id))
            .Select(mapper.Map<HeroBasicViewModel>)
            .OrderBy(h => h.Name)
            .ToList();
    }

    public async Task<IReadOnlyCollection<HeroBasicViewModel>> GetAddableHeroesForTeamAsync(string profileId,
        string teamId)
    {
        if (!await EnsureInitializedAsync(profileId))
        {
            return [];
        }

        var heroIds = commandCenterUiService.CurrentProfile!.Heroes.Values.Where(h => !commandCenterUiService.CurrentProfile!.Teams[teamId].HeroProfileIds.Contains(h.Id))
            .Select(p => p.HeroId).ToHashSet();
        return commandCenterUiService.Heroes.Values
            .Where(h => heroIds.Contains(h.Id))
            .Select(mapper.Map<HeroBasicViewModel>)
            .OrderBy(h => h.Name)
            .ToList();
    }

    public async Task<CcProfileTeamViewModel?> GetTeamAsync(string profileId, string teamId)
    {
        if (!await EnsureInitializedAsync(profileId))
        {
            return null;
        }

        if (!commandCenterUiService.CurrentProfile!.Teams.TryGetValue(teamId, out var team))
        {
            return null;
        }

        return profileTeamViewModelFactory.Create(team, _heroProfileViewModels.AsReadOnly());
    }

    public async Task RemoveHeroFromTeamAsync(string profileId, string teamId, string heroProfileId)
    {
        if (!await EnsureInitializedAsync(profileId))
        {
            return;
        }

        commandCenterUiService.CurrentProfile!.Teams[teamId].HeroProfileIds.Remove(heroProfileId);
        await SaveCurrentProfile();
        NotifyStateChanged();
    }

    public async Task<string?> CreateTeamAsync(string profileId, string teamName)
    {
        if (!await EnsureInitializedAsync(profileId))
        {
            return null;
        }

        var team = commandCenterTeamFactory.Create(teamName);
        commandCenterUiService.CurrentProfile!.Teams.Add(team.Id, team);
        await SaveCurrentProfile();
        NotifyStateChanged();
        return team.Id;
    }

    public async Task AddHeroToTeamAsync(string profileId, string teamId, string heroId)
    {
        if (!await EnsureInitializedAsync(profileId))
        {
            return;
        }

        var heroProfile = commandCenterUiService.CurrentProfile!.Heroes.Values.FirstOrDefault(hp => hp.HeroId == heroId);
        if (heroProfile == null)
        {
            return;
        }

        if (!commandCenterUiService.CurrentProfile.Teams.TryGetValue(teamId, out var team))
        {
            return;
        }

        if (team.HeroProfileIds.Contains(heroProfile.Id) || team.HeroProfileIds.Count >= HohConstants.MAX_TEAM_MEMBERS)
        {
            return;
        }

        team.HeroProfileIds.Add(heroProfile.Id);
        await SaveCurrentProfile();

        NotifyStateChanged();
    }

    public async Task<HeroProfileViewModel?> GetHeroProfileAsync(string profileId, string heroProfileId)
    {
        if (!await EnsureInitializedAsync(profileId))
        {
            return null;
        }

        return _heroProfileViewModels.TryGetValue(heroProfileId, out var heroProfile) ? heroProfile : null;
    }

    public HeroProfileViewModel? UpdateHeroProfile(HeroProfileStatsUpdateRequest request)
    {
        if (commandCenterUiService.CurrentProfile == null || !commandCenterUiService.CurrentProfile.Heroes.TryGetValue(request.HeroProfileId, out var heroProfile))
        {
            return null;
        }

        heroProfile!.Level = request.Level.Level;
        heroProfile.AscensionLevel = request.Level.AscensionLevel;
        heroProfile.AbilityLevel = request.AbilityLevel;
        heroProfile.AwakeningLevel = request.AwakeningLevel;
        heroProfile.BarracksLevel = request.BarracksLevel;

        var hero = commandCenterUiService.Heroes[heroProfile.HeroId];
        var group = hero.Unit.Type.ToBuildingGroup();
        var heroBarracks = commandCenterUiService.CommandCenterData.Barracks.First(b =>
            b.Group == group && b.Level == request.BarracksLevel);
        var newViewModel = UpdateHeroProfile(heroProfile, hero, heroBarracks);

        Task.Run(async () => { await SaveCurrentProfile(); });

        return newViewModel;
    }
    
    private HeroProfileViewModel UpdateHeroProfile(HeroProfile heroProfile, HeroDto hero, BuildingDto barracks)
    {
        var newHeroProfile = heroProfileFactory.Create(heroProfile, hero, barracks);
        var newViewModel = CreateHeroProfileViewModel(newHeroProfile);
        commandCenterUiService.CurrentProfile!.Heroes.Remove(heroProfile.Id);
        commandCenterUiService.CurrentProfile.Heroes.Add(newHeroProfile.Id, newHeroProfile);
        _heroProfileViewModels.Remove(heroProfile.Id);
        _heroProfileViewModels.Add(newHeroProfile.Id, newViewModel);
        return newViewModel;
    }

    private HeroProfileViewModel CreateHeroProfileViewModel(HeroProfile heroProfile)
    {
        var hero = commandCenterUiService.Heroes[heroProfile.HeroId];
        return heroProfileViewModelFactory.CreateForCommandCenterProfile(heroProfile, hero);
    }
    
    private async Task<bool> EnsureInitializedAsync(string profileId)
    {
        if (commandCenterUiService.CurrentProfile != null && commandCenterUiService.CurrentProfile.Id == profileId)
        {
            return true;
        }

        await commandCenterUiService.EnsureInitializedAsync();

        commandCenterUiService.CurrentProfile = null;
        _heroProfileViewModels.Clear();

        var profileDto = await persistenceService.LoadProfile(profileId);

        if (profileDto == null)
        {
            return false;
        }

        var profile = commandCenterProfileFactory.Create(profileDto, commandCenterUiService.Heroes,
            commandCenterUiService.CommandCenterData.Barracks);

        var profileViewModels = profile.Heroes.Values.Select(CreateHeroProfileViewModel).ToList();
        commandCenterUiService.CurrentProfile = profile;
        _heroProfileViewModels = profileViewModels.ToDictionary(hp => hp.Id);
        return true;
    }

    private void NotifyStateChanged()
    {
        StateHasChanged?.Invoke();
    }

    private ValueTask SaveCurrentProfile()
    {
        return persistenceService.SaveProfile(mapper.Map<BasicCommandCenterProfile>(commandCenterUiService.CurrentProfile));
    }
}
