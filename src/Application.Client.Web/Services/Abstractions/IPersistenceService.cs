using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Services.Abstractions;

public interface IPersistenceService
{
    ValueTask SaveCity(HohCity city);
    ValueTask SaveCityInspirationsRequestAsync(CityInspirationsSearchFormRequest request);
    ValueTask<CityInspirationsSearchFormRequest?> GetCityInspirationsRequestAsync();
    ValueTask SaveTopHeroesRequestAsync(TopHeroesSearchFormRequest request);
    ValueTask<TopHeroesSearchFormRequest?> GetTopHeroesRequestAsync();
    ValueTask<bool> DeleteCity(string cityId);
    ValueTask<HohCity?> LoadCity(string cityId);
    ValueTask<IReadOnlyCollection<HohCityBasicData>> GetCities();

    ValueTask SaveCommandCenterProfile(BasicCommandCenterProfile commandCenterProfile);
    ValueTask SaveEquipment(IReadOnlyCollection<EquipmentItem> equipment);
    ValueTask<bool> DeleteProfile(string profileId);
    ValueTask<BasicCommandCenterProfile?> LoadProfile(string profileId);
    ValueTask<IReadOnlyCollection<BasicCommandCenterProfile>> GetProfilesAsync();
    ValueTask<HeroProfileIdentifier?> GetHeroProfileAsync(string heroId);
    ValueTask<IReadOnlyCollection<EquipmentItem>> GetEquipmentAsync();
    ValueTask SaveHeroProfileAsync(HeroProfileIdentifier profile);

    ValueTask<UiSettings> GetUiSettingsAsync();
    ValueTask SaveUiSettingsAsync(UiSettings settings);
    ValueTask SaveTempCities(IEnumerable<HohCity> cities);
    ValueTask<IReadOnlyCollection<HohCity>> GetTempCities();
    ValueTask SaveCityBackup(HohCityBackup cityBackup);
    ValueTask SaveCommandCenterProfileBackup(CommandCenterProfileBackup backup);

    ValueTask SaveOpenTechnologies(CityId cityId, IReadOnlyCollection<string> openTechnologies);
    ValueTask<IReadOnlyCollection<string>> GetOpenTechnologies(CityId cityId);
    
    ValueTask SetItemAsync<T>(string key, T value);
    ValueTask<T?> GetItemAsync<T>(string key);
}
