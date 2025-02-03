using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter;

public class CcHeroesPlaygroundUiService(
    ICommandCenterUiService commandCenterUiService,
    IPersistenceService persistenceService,
    IHohHeroProfileViewModelFactory heroProfileViewModelFactory,
    IHohHeroProfileFactory heroProfileFactory,
    IHohHeroProfileDtoFactory heroProfileDtoFactory,
    IMapper mapper) : ICcHeroesPlaygroundUiService
{
    public event Action? StateHasChanged;
    private IDictionary<string, HeroProfile>? _profiles;
    private IDictionary<string, HeroProfileViewModel> _viewModels = null!;

    public async Task<HeroProfileViewModel?> GetHeroProfileAsync(string heroProfileId)
    {
        await EnsureInitializedAsync();

        return _viewModels.TryGetValue(heroProfileId, out var heroProfile) ? heroProfile : null;
    }

    public HeroProfileViewModel? UpdateHeroProfile(HeroProfileStatsUpdateRequest request)
    {
        if (_profiles == null || !_profiles.TryGetValue(request.HeroProfileId, out var profile))
        {
            return null;
        }

        profile!.Level = request.Level.Level;
        profile.AscensionLevel = request.Level.AscensionLevel;
        profile.AbilityLevel = request.AbilityLevel;
        profile.AwakeningLevel = request.AwakeningLevel;
        profile.BarracksLevel = request.BarracksLevel;

        var hero = commandCenterUiService.Heroes[profile.HeroId];
        var group = hero.Unit.Type.ToBuildingGroup();
        var heroBarracks =
            commandCenterUiService.CommandCenterData.Barracks.First(b =>
                b.Group == group && b.Level == request.BarracksLevel);
        var newHeroProfile = heroProfileFactory.Create(profile, hero, heroBarracks);
        var newViewModel = CreateHeroProfileViewModel(newHeroProfile);
        _profiles.Remove(profile.Id);
        _profiles.Add(newHeroProfile.Id, newHeroProfile);
        _viewModels.Remove(profile.Id);
        _viewModels.Add(newHeroProfile.Id, newViewModel);
        Task.Run(async () =>
        {
            await persistenceService.SaveHeroPlaygroundProfilesAsync(
                mapper.Map<IReadOnlyDictionary<string, HeroPlaygroundProfile>>(_profiles));
        });

        return newViewModel;
    }

    public async Task<IReadOnlyCollection<HeroProfileViewModel>> GetHeroes()
    {
        await EnsureInitializedAsync();
        return _viewModels.Values.ToList();
    }

    private HeroProfileViewModel CreateHeroProfileViewModel(HeroProfile profile)
    {
        var hero = commandCenterUiService.Heroes[profile.HeroId];
        var group = hero.Unit.Type.ToBuildingGroup();
        var barracks = commandCenterUiService.CommandCenterData.Barracks.Where(b => b.Group == group).ToList();
        return heroProfileViewModelFactory.CreateForPlayground(profile, hero, barracks);
    }

    private async Task EnsureInitializedAsync()
    {
        if (_profiles != null)
        {
            return;
        }

        await commandCenterUiService.EnsureInitializedAsync();

        var existingProfileDtos = await persistenceService.GetHeroPlaygroundProfilesAsync();
        var absentHeroes = commandCenterUiService.Heroes.Values
            .Where(hd => !existingProfileDtos.ContainsKey(hd.Id)).ToList();
        var allProfileDtos = new List<HeroPlaygroundProfile>(existingProfileDtos.Values);
        if (absentHeroes.Count > 0)
        {
            var newHeroProfiles = absentHeroes.Select(hd =>
            {
                var group = hd.Unit.Type.ToBuildingGroup();
                var barracksLevel = commandCenterUiService.CommandCenterData.Barracks.Where(b => b.Group == group)
                    .OrderBy(b => b.Level)
                    .First().Level;
                return heroProfileDtoFactory.Create(hd.Id, hd.Id, barracksLevel);
            });
            allProfileDtos = allProfileDtos.Concat(newHeroProfiles).ToList();
            await persistenceService.SaveHeroPlaygroundProfilesAsync(allProfileDtos.ToDictionary(src => src.Id));
        }

        var profiles = new Dictionary<string, HeroProfile>(StringComparer.OrdinalIgnoreCase);
        foreach (var profileDto in allProfileDtos)
        {
            var hero = commandCenterUiService.Heroes[profileDto.Id];
            var group = hero.Unit.Type.ToBuildingGroup();
            var barracks =
                commandCenterUiService.CommandCenterData.Barracks.First(b =>
                    b.Group == group && b.Level == profileDto.BarracksLevel);
            profiles.Add(profileDto.Id, heroProfileFactory.Create(profileDto, hero, barracks));
        }

        var profileViewModels = new List<HeroProfileViewModel>();
        foreach (var profile in profiles.Values)
        {
            profileViewModels.Add(CreateHeroProfileViewModel(profile));
        }

        _profiles = profiles;
        _viewModels = profileViewModels.ToDictionary(hp => hp.Id);
    }

    private void NotifyStateChanged()
    {
        StateHasChanged?.Invoke();
    }
}
