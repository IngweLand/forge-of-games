using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;

public interface ICcProfileUiService
{
    event Action? StateHasChanged;
    Task AddHeroAsync(string profileId, string heroId);
    Task AddHeroToTeamAsync(string profileId, string teamId, string heroId);
    Task<string?> CreateTeamAsync(string profileId, string teamName);
    Task DeleteTeamAsync(string profileId, string teamId);
    Task<IReadOnlyCollection<HeroBasicViewModel>> GetAddableHeroesForProfileAsync(string profileId);
    Task<IReadOnlyCollection<HeroBasicViewModel>> GetAddableHeroesForTeamAsync(string profileId, string teamId);
    Task<IReadOnlyCollection<CcBarracksViewModel>> GetBarracks(string profileId);
    Task<HeroProfileViewModel?> GetHeroProfileAsync(string profileId, string heroProfileId);
    Task<CcProfileViewModel?> GetProfileAsync(string profileId);
    Task<BasicCommandCenterProfile?> GetProfileDtoAsync(string profileId);
    Task<IReadOnlyCollection<HeroProfileViewModel>> GetProfileHeroesAsync(string profileId);
    Task<CcProfileTeamViewModel?> GetTeamAsync(string profileId, string teamId);
    Task RemoveHeroFromProfileAsync(string profileId, string heroProfileId);
    Task RemoveHeroFromTeamAsync(string profileId, string teamId, string heroProfileId);
    Task<bool> UpdateProfileSettingsAsync(string profileId, CcProfileSettings profileSettings);
    Task UpdateBarracks(string profileId, CcBarracksViewModel barracks);
    HeroProfileViewModel? UpdateHeroProfile(HeroProfileStatsUpdateRequest request);
    Task<CcProfileSettings?> GetSettingsAsync(string profileId);
}
