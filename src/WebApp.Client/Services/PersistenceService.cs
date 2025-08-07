using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Blazored.LocalStorage;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.WebApp.Client.Services;

public class PersistenceService(ILocalStorageService localStorageService, IMapper mapper) : IPersistenceService
{
    private const string CITY_DATA_KEY_PREFIX = "CityData";
    private const string BACKUP_CITY_DATA_KEY_PREFIX = "Backup.CityData";
    private const string BACKUP_COMMAND_CENTER_PROFILE_KEY_PREFIX = "Backup.CommandCenterProfile";
    private const string TEMP_CITY_DATA_KEY_PREFIX = "TEMP.CityData";
    private const string PROFILE_DATA_KEY_PREFIX = "CommandCenterProfile";
    private const string HERO_PLAYGROUND_PROFILES_DATA_KEY_PREFIX = "HeroPlaygroundProfilesData";
    private const string EQUIPMENT_DATA_KEY_PREFIX = "Equipment";
    private const string UI_SETTINGS = "UiSettings";
    private const string CITY_INSPIRATIONS_REQUEST = "CityInspirationsRequest";
    private const string TOP_HEROES_REQUEST = "TopHeroesRequest";
    private const string OPEN_TECHNOLOGIES = "OpenTechnologies";
    public const string SUBMISSION_ID_KEY = "SubmissionId";

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = {new JsonStringEnumConverter()},
    };

    public ValueTask SaveCity(HohCity city)
    {
        return DoSaveCity(GetCityKey(city.Id), city);
    }

    public ValueTask SaveCityInspirationsRequestAsync(CityInspirationsSearchFormRequest request)
    {
        var serializedRequest = JsonSerializer.Serialize(request, JsonSerializerOptions);
        return localStorageService.SetItemAsStringAsync(CITY_INSPIRATIONS_REQUEST, serializedRequest);
    }

    public async ValueTask<CityInspirationsSearchFormRequest?> GetCityInspirationsRequestAsync()
    {
        var rawData = await localStorageService.GetItemAsStringAsync(CITY_INSPIRATIONS_REQUEST);
        return !string.IsNullOrWhiteSpace(rawData)
            ? JsonSerializer.Deserialize<CityInspirationsSearchFormRequest>(rawData, JsonSerializerOptions)
            : null;
    }

    public ValueTask SaveTopHeroesRequestAsync(TopHeroesSearchFormRequest request)
    {
        var serializedRequest = JsonSerializer.Serialize(request, JsonSerializerOptions);
        return localStorageService.SetItemAsStringAsync(TOP_HEROES_REQUEST, serializedRequest);
    }

    public async ValueTask<TopHeroesSearchFormRequest?> GetTopHeroesRequestAsync()
    {
        var rawData = await localStorageService.GetItemAsStringAsync(TOP_HEROES_REQUEST);
        if (string.IsNullOrWhiteSpace(rawData))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<TopHeroesSearchFormRequest>(rawData, JsonSerializerOptions);
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public ValueTask SaveCityBackup(HohCityBackup cityBackup)
    {
        var serializedCity = JsonSerializer.Serialize(cityBackup, JsonSerializerOptions);
        return localStorageService.SetItemAsStringAsync(GetCityBackupKey(cityBackup.City.Id), serializedCity);
    }

    public ValueTask SaveCommandCenterProfileBackup(CommandCenterProfileBackup backup)
    {
        var serializedCity = JsonSerializer.Serialize(backup, JsonSerializerOptions);
        return localStorageService.SetItemAsStringAsync(GetCommandCenterProfileBackupKey(backup.Profile.Id),
            serializedCity);
    }

    public async ValueTask SaveOpenTechnologies(CityId cityId, IReadOnlyCollection<string> openTechnologies)
    {
        var allTechs = await GetAllOpenTechnologiesAsync();
        allTechs[cityId] = openTechnologies;
        var serializedCity = JsonSerializer.Serialize(allTechs, JsonSerializerOptions);
        await localStorageService.SetItemAsStringAsync(OPEN_TECHNOLOGIES, serializedCity);
    }

    public async ValueTask<IReadOnlyCollection<string>> GetOpenTechnologies(CityId cityId)
    {
        var allTechs = await GetAllOpenTechnologiesAsync();
        return allTechs.GetValueOrDefault(cityId, []);
    }

    public ValueTask SetItemAsync<T>(string key, T value)
    {
        return localStorageService.SetItemAsync(key, value);
    }

    public ValueTask<T?> GetItemAsync<T>(string key)
    {
        return localStorageService.GetItemAsync<T>(key);
    }

    public ValueTask SaveSubmissionId(Guid submissionId)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Guid?> GetSubmissionId()
    {
        throw new NotImplementedException();
    }

    public async ValueTask<bool> DeleteCity(string cityId)
    {
        var key = GetCityKey(cityId);
        if (!await localStorageService.ContainKeyAsync(key))
        {
            return false;
        }

        try
        {
            await localStorageService.RemoveItemAsync(key);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public ValueTask<HohCity?> LoadCity(string cityId)
    {
        return DoLoadCity(GetCityKey(cityId));
    }

    public async ValueTask<IReadOnlyCollection<HohCityBasicData>> GetCities()
    {
        var keys = await localStorageService.KeysAsync();
        var cityKeys = keys.Where(s => s.StartsWith(CITY_DATA_KEY_PREFIX));
        var cities = new List<HohCityBasicData>();
        foreach (var cityKey in cityKeys)
        {
            var city = await DoLoadCity(cityKey);
            if (city != null)
            {
                cities.Add(mapper.Map<HohCityBasicData>(city));
            }
        }

        return cities.OrderByDescending(x => x.UpdatedAt).ToList();
    }

    public ValueTask SaveCommandCenterProfile(BasicCommandCenterProfile commandCenterProfile)
    {
        var serializedProfile = JsonSerializer.Serialize(commandCenterProfile);
        return localStorageService.SetItemAsStringAsync(GetProfileKey(commandCenterProfile.Id), serializedProfile);
    }

    public ValueTask SaveEquipment(IReadOnlyCollection<EquipmentItem> equipment)
    {
        var serializedProfile = JsonSerializer.Serialize(equipment, JsonSerializerOptions);
        return localStorageService.SetItemAsStringAsync(EQUIPMENT_DATA_KEY_PREFIX, serializedProfile);
    }

    public async ValueTask<bool> DeleteProfile(string profileId)
    {
        var key = GetProfileKey(profileId);
        if (!await localStorageService.ContainKeyAsync(key))
        {
            return false;
        }

        try
        {
            await localStorageService.RemoveItemAsync(key);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public ValueTask<BasicCommandCenterProfile?> LoadProfile(string profileId)
    {
        return DoLoadProfile(GetProfileKey(profileId));
    }

    public async ValueTask<IReadOnlyCollection<BasicCommandCenterProfile>> GetProfilesAsync()
    {
        var keys = await localStorageService.KeysAsync();
        var profileKeys = keys.Where(s => s.StartsWith(PROFILE_DATA_KEY_PREFIX));
        var profiles = new List<BasicCommandCenterProfile>();
        foreach (var profileKey in profileKeys)
        {
            var profile = await DoLoadProfile(profileKey);
            if (profile != null)
            {
                profiles.Add(profile);
            }
        }

        return profiles;
    }

    public async ValueTask<HeroProfileIdentifier?> GetHeroProfileAsync(string heroId)
    {
        var profiles = await GetHeroProfilesAsync();

        profiles.TryGetValue(heroId, out var profile);
        return profile;
    }

    public async ValueTask<IReadOnlyCollection<EquipmentItem>> GetEquipmentAsync()
    {
        List<EquipmentItem>? equipment = null;
        var rawData = await localStorageService.GetItemAsStringAsync(EQUIPMENT_DATA_KEY_PREFIX);
        if (!string.IsNullOrWhiteSpace(rawData))
        {
            equipment = JsonSerializer.Deserialize<List<EquipmentItem>>(rawData, JsonSerializerOptions);
        }

        return equipment ?? [];
    }

    public async ValueTask<UiSettings> GetUiSettingsAsync()
    {
        var rawData = await localStorageService.GetItemAsStringAsync(UI_SETTINGS);
        UiSettings? settings = null;
        try
        {
            settings = string.IsNullOrWhiteSpace(rawData)
                ? new UiSettings()
                : JsonSerializer.Deserialize<UiSettings>(rawData);
        }
        catch (Exception e)
        {
            // ignore
        }

        return settings ?? new UiSettings();
    }

    public ValueTask SaveUiSettingsAsync(UiSettings settings)
    {
        var serializedSettings = JsonSerializer.Serialize(settings);
        return localStorageService.SetItemAsStringAsync(UI_SETTINGS, serializedSettings);
    }

    public async ValueTask SaveTempCities(IEnumerable<HohCity> cities)
    {
        await DeleteAllTempCities();
        foreach (var city in cities)
        {
            await DoSaveCity(GetTempCityKey(city.Id), city);
        }
    }

    public async ValueTask<IReadOnlyCollection<HohCity>> GetTempCities()
    {
        var keys = await localStorageService.KeysAsync();
        var cityKeys = keys.Where(s => s.StartsWith(TEMP_CITY_DATA_KEY_PREFIX));
        var cities = new List<HohCity>();
        foreach (var cityKey in cityKeys)
        {
            var city = await DoLoadCity(cityKey);
            if (city != null)
            {
                cities.Add(city);
            }
        }

        return cities;
    }

    public async ValueTask SaveHeroProfileAsync(HeroProfileIdentifier profile)
    {
        var profiles = await GetHeroProfilesAsync();
        profiles[profile.HeroId] = profile;
        var serializedProfile = JsonSerializer.Serialize(profiles);
        await localStorageService.SetItemAsStringAsync(HERO_PLAYGROUND_PROFILES_DATA_KEY_PREFIX, serializedProfile);
    }

    private async ValueTask<Dictionary<string, HeroProfileIdentifier>> GetHeroProfilesAsync()
    {
        var rawData = await localStorageService.GetItemAsStringAsync(HERO_PLAYGROUND_PROFILES_DATA_KEY_PREFIX);
        if (!string.IsNullOrWhiteSpace(rawData))
        {
            return JsonSerializer.Deserialize<Dictionary<string, HeroProfileIdentifier>>(rawData) ?? [];
        }

        return [];
    }

    private async ValueTask<Dictionary<CityId, IReadOnlyCollection<string>>> GetAllOpenTechnologiesAsync()
    {
        var rawData = await localStorageService.GetItemAsStringAsync(OPEN_TECHNOLOGIES);
        if (!string.IsNullOrWhiteSpace(rawData))
        {
            return JsonSerializer.Deserialize<Dictionary<CityId, IReadOnlyCollection<string>>>(rawData) ?? [];
        }

        return [];
    }

    private async Task DeleteAllTempCities()
    {
        var keys = await localStorageService.KeysAsync();
        var cityKeys = keys.Where(s => s.StartsWith(TEMP_CITY_DATA_KEY_PREFIX));
        foreach (var key in cityKeys)
        {
            try
            {
                await localStorageService.RemoveItemAsync(key);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    private ValueTask DoSaveCity(string key, HohCity city)
    {
        if (city.Entities.Count > 0)
        {
            var nextId = city.Entities.Max(cme => cme.Id) + 1;
            foreach (var hohCityMapEntity in city.Entities)
            {
                if (hohCityMapEntity.Id < 0)
                {
                    hohCityMapEntity.Id = nextId;
                    nextId++;
                }
            }
        }

        var serializedCity = JsonSerializer.Serialize(city, JsonSerializerOptions);
        return localStorageService.SetItemAsStringAsync(key, serializedCity);
    }

    private async ValueTask<HohCity?> DoLoadCity(string key)
    {
        var rawData = await localStorageService.GetItemAsStringAsync(key);
        return string.IsNullOrWhiteSpace(rawData)
            ? null
            : JsonSerializer.Deserialize<HohCity>(rawData, JsonSerializerOptions);
    }

    private async ValueTask<BasicCommandCenterProfile?> DoLoadProfile(string key)
    {
        var rawData = await localStorageService.GetItemAsStringAsync(key);
        return string.IsNullOrWhiteSpace(rawData)
            ? null
            : JsonSerializer.Deserialize<BasicCommandCenterProfile>(rawData);
    }

    private static string GetCityKey(string id)
    {
        return $"{CITY_DATA_KEY_PREFIX}.{id}";
    }

    private static string GetCityBackupKey(string id)
    {
        return $"{BACKUP_CITY_DATA_KEY_PREFIX}.{id}";
    }

    private static string GetCommandCenterProfileBackupKey(string id)
    {
        return $"{BACKUP_COMMAND_CENTER_PROFILE_KEY_PREFIX}.{id}";
    }

    private static string GetTempCityKey(string id)
    {
        return $"{TEMP_CITY_DATA_KEY_PREFIX}.{id}";
    }

    private static string GetProfileKey(string id)
    {
        return $"{PROFILE_DATA_KEY_PREFIX}.{id}";
    }
}
