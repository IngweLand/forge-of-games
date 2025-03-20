using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Services.Abstractions;

public interface IPersistenceService
{
    ValueTask SaveCity(HohCity city);
    ValueTask<bool> DeleteCity(string cityId);
    ValueTask<HohCity?> LoadCity(string cityId);
    ValueTask<IReadOnlyCollection<HohCityBasicData>> GetCities();

    ValueTask SaveProfile(BasicCommandCenterProfile commandCenterProfile);
    ValueTask<bool> DeleteProfile(string profileId);
    ValueTask<BasicCommandCenterProfile?> LoadProfile(string profileId);
    ValueTask<IReadOnlyCollection<BasicCommandCenterProfile>> GetProfilesAsync();
    ValueTask<IReadOnlyDictionary<string, HeroPlaygroundProfile>> GetHeroPlaygroundProfilesAsync();
    ValueTask SaveHeroPlaygroundProfilesAsync(IReadOnlyDictionary<string, HeroPlaygroundProfile> profiles);
    
    ValueTask<UiSettings> GetUiSettingsAsync();
    ValueTask SaveUiSettingsAsync(UiSettings settings);
}