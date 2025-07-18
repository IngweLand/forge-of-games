using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;

public interface ICcProfileUiService
{
    event Action? StateHasChanged;
    BasicCommandCenterProfile GetProfileDto();
    Task<bool> UpdateProfileSettingsAsync(CcProfileSettings profileSettings);
    IReadOnlyCollection<CcBarracksViewModel> GetBarracks();
    Task UpdateBarracksAsync(CcBarracksViewModel barracks);
    CcProfileSettings GetSettings();
    Task DeleteTeamAsync(string teamId);
    IReadOnlyCollection<HeroBasicViewModel> GetAddableHeroesForProfile();
    IReadOnlyCollection<HeroBasicViewModel> GetAddableHeroesForTeam(string teamId);
    CcProfileTeamViewModel? GetTeam(string teamId);
    Task RemoveHeroFromTeamAsync(string teamId, string heroProfileId);
    Task<string?> CreateTeamAsync(string teamName);
    Task AddHeroToTeamAsync(string teamId, string heroId);

    Task<HeroProfileIdentifier> GetHeroProfileIdentifierAsync(string heroId);
    Task UpdateHeroProfileAsync(HeroProfileIdentifier identifier);
    Task<CcProfileViewModel?> InitializedAsync(string profileId);
    CcProfileViewModel GetCurrentProfile();
    Task<IReadOnlyCollection<HeroProfileViewModel>> GetProfileHeroesAsync();
    Task AddHeroAsync(string heroId);
    Task RemoveHeroFromProfileAsync(string heroId);
}
