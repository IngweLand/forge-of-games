using System.Text.Json;
using Blazored.LocalStorage;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.WebApp.Client.Services;

public class PersistenceService(ILocalStorageService localStorageService) : IPersistenceService
{
    private const string CITY_DATA_KEY_PREFIX = "CityData";
    private const string PROFILE_DATA_KEY_PREFIX = "CommandCenterProfile";
    private const string HERO_PLAYGROUND_PROFILES_DATA_KEY_PREFIX = "HeroPlaygroundProfilesData";

    public ValueTask SaveCity(HohCity city)
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

        var serializedCity = JsonSerializer.Serialize(city);
        return localStorageService.SetItemAsStringAsync(GetCityKey(city.Id), serializedCity);
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
                cities.Add(new HohCityBasicData()
                {
                    Id = city.Id,
                    Name = city.Name,
                });
            }
        }

        return cities;
    }

    public ValueTask SaveProfile(BasicCommandCenterProfile commandCenterProfile)
    {
        var serializedProfile = JsonSerializer.Serialize(commandCenterProfile);
        return localStorageService.SetItemAsStringAsync(GetProfileKey(commandCenterProfile.Id), serializedProfile);
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
    
    public async ValueTask<IReadOnlyDictionary<string, HeroPlaygroundProfile>> GetHeroPlaygroundProfilesAsync()
    {
        Dictionary<string, HeroPlaygroundProfile>? profiles = null;
        var rawData = await localStorageService.GetItemAsStringAsync(HERO_PLAYGROUND_PROFILES_DATA_KEY_PREFIX);
        if(!string.IsNullOrWhiteSpace(rawData))
        {
            profiles = JsonSerializer.Deserialize<Dictionary<string, HeroPlaygroundProfile>>(rawData);
        }

        return profiles ?? [];
    }
    
    public ValueTask SaveHeroPlaygroundProfilesAsync(IReadOnlyDictionary<string, HeroPlaygroundProfile> profiles)
    {
        var serializedProfile = JsonSerializer.Serialize(profiles);
        return localStorageService.SetItemAsStringAsync(HERO_PLAYGROUND_PROFILES_DATA_KEY_PREFIX, serializedProfile);
    }

    private async ValueTask<HohCity?> DoLoadCity(string key)
    {
        var rawData = await localStorageService.GetItemAsStringAsync(key);
        return string.IsNullOrWhiteSpace(rawData) ? null : JsonSerializer.Deserialize<HohCity>(rawData);
    }

    private async ValueTask<BasicCommandCenterProfile?> DoLoadProfile(string key)
    {
        var rawData = await localStorageService.GetItemAsStringAsync(key);
        return string.IsNullOrWhiteSpace(rawData) ? null : JsonSerializer.Deserialize<BasicCommandCenterProfile>(rawData);
    }

    private static string GetCityKey(string id)
    {
        return $"{CITY_DATA_KEY_PREFIX}.{id}";
    }

    private static string GetProfileKey(string id)
    {
        return $"{PROFILE_DATA_KEY_PREFIX}.{id}";
    }
}
