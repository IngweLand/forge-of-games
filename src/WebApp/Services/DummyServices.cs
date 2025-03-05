using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.WebApp.Client.Models;
using Ingweland.Fog.WebApp.Client.Services;
using Ingweland.Fog.WebApp.Client.Services.Abstractions;

namespace Ingweland.Fog.WebApp.Services;

internal class DummyPersistenceService : IPersistenceService
{
    public ValueTask SaveCity(HohCity city)
    {
        throw new NotImplementedException();
    }

    public ValueTask<HohCity?> LoadCity(string cityId)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IReadOnlyCollection<HohCityBasicData>> GetCities()
    {
        throw new NotImplementedException();
    }

    public ValueTask SaveProfile(BasicCommandCenterProfile commandCenterProfile)
    {
        throw new NotImplementedException();
    }

    public ValueTask<bool> DeleteProfile(string profileId)
    {
        throw new NotImplementedException();
    }

    public ValueTask<BasicCommandCenterProfile?> LoadProfile(string profileId)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IReadOnlyCollection<BasicCommandCenterProfile>> GetProfilesAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask<IReadOnlyDictionary<string, HeroPlaygroundProfile>> GetHeroPlaygroundProfilesAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask SaveHeroPlaygroundProfilesAsync(IReadOnlyDictionary<string, HeroPlaygroundProfile> profiles)
    {
        throw new NotImplementedException();
    }
}

internal class DummyClientLocaleService : IClientLocaleService
{
    public Task<LocaleInfo> GetCurrentLocaleAsync()
    {
        throw new NotImplementedException();
    }

    public Task SetLocaleAsync(LocaleInfo locale)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<LocaleInfo> SupportedLocales => throw new NotImplementedException();
}

internal class DummyClipboardService : IClipboardService
{
    public ValueTask<string> ReadTextAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask WriteTextAsync(string text)
    {
        throw new NotImplementedException();
    }
}

internal class DummyJSInteropService : IJSInteropService
{
    public ValueTask ResetScrollPositionAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask ShowLoadingIndicatorAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask<bool> IsMobileAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask OpenUrlAsync(string url, string target)
    {
        throw new NotImplementedException();
    }

    public ValueTask HideLoadingIndicatorAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask<bool> CopyToClipboardAsync(string payload)
    {
        throw new NotImplementedException();
    }
}

internal class DummyInGameStartupDataService : IInGameStartupDataService
{
    public Task<ResourceCreatedResponse> ImportInGameDataAsync(ImportInGameStartupDataRequestDto importHeroesRequestDto)
    {
        throw new NotImplementedException();
    }

    public Task<InGameStartupData?> GetImportedInGameDataAsync(string inGameStartupDataId)
    {
        throw new NotImplementedException();
    }
}

internal class DummyCityPlannerSharingService : ICityPlannerSharingService
{
    public Task<ResourceCreatedResponse> ShareAsync(HohCity city)
    {
        throw new NotImplementedException();
    }

    public Task<HohCity?> GetSharedCityAsync(string cityId)
    {
        throw new NotImplementedException();
    }
}
