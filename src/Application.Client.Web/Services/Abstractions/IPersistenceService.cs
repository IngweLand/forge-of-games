using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Dtos.Hoh.PlayerCity;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;

namespace Ingweland.Fog.Application.Client.Web.Services.Abstractions;

public interface IPersistenceService
{
    ValueTask SaveCity(HohCity city);
    ValueTask SaveCityInspirationsRequestAsync(CityInspirationsSearchFormRequest request);
    ValueTask<CityInspirationsSearchFormRequest?> GetCityInspirationsRequestAsync();
    ValueTask<bool> DeleteCity(string cityId);
    ValueTask<HohCity?> LoadCity(string cityId);
    ValueTask<IReadOnlyCollection<HohCityBasicData>> GetCities();

    ValueTask SaveProfile(BasicCommandCenterProfile commandCenterProfile);
    ValueTask SaveEquipment(IReadOnlyCollection<EquipmentItem> equipment);
    ValueTask<bool> DeleteProfile(string profileId);
    ValueTask<BasicCommandCenterProfile?> LoadProfile(string profileId);
    ValueTask<IReadOnlyCollection<BasicCommandCenterProfile>> GetProfilesAsync();
    ValueTask<IReadOnlyDictionary<string, HeroPlaygroundProfile>> GetHeroPlaygroundProfilesAsync();
    ValueTask<IReadOnlyCollection<EquipmentItem>> GetEquipmentAsync();
    ValueTask SaveHeroPlaygroundProfilesAsync(IReadOnlyDictionary<string, HeroPlaygroundProfile> profiles);

    ValueTask<UiSettings> GetUiSettingsAsync();
    ValueTask SaveUiSettingsAsync(UiSettings settings);
    ValueTask SaveTempCities(IEnumerable<HohCity> cities);
    ValueTask<IReadOnlyCollection<HohCity>> GetTempCities();
    ValueTask SaveCityBackup(HohCityBackup cityBackup);
}
