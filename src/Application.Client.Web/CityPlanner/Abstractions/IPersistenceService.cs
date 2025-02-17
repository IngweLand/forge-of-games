using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IPersistenceService
{
    ValueTask SaveCity(HohCity city);
    ValueTask<HohCity?> LoadCity(string cityId);
    ValueTask<IReadOnlyCollection<HohCityBasicData>> GetCities();

    ValueTask SaveProfile(BasicCommandCenterProfile commandCenterProfile);
    ValueTask<bool> DeleteProfile(string profileId);
    ValueTask<BasicCommandCenterProfile?> LoadProfile(string profileId);
    ValueTask<IReadOnlyCollection<BasicCommandCenterProfile>> GetProfilesAsync();
    ValueTask<IReadOnlyDictionary<string, HeroPlaygroundProfile>> GetHeroPlaygroundProfilesAsync();
    ValueTask SaveHeroPlaygroundProfilesAsync(IReadOnlyDictionary<string, HeroPlaygroundProfile> profiles);
}
