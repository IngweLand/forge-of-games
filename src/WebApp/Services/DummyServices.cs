using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.WebApp.Client.Models;
using Ingweland.Fog.WebApp.Client.Services.Abstractions;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Services;

internal class DummyPersistenceService : IPersistenceService
{
    public ValueTask SaveCity(HohCity city)
    {
        throw new NotImplementedException();
    }

    public ValueTask SaveCityInspirationsRequestAsync(CityInspirationsSearchFormRequest request)
    {
        throw new NotImplementedException();
    }

    public ValueTask<CityInspirationsSearchFormRequest?> GetCityInspirationsRequestAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask SaveTopHeroesRequestAsync(TopHeroesSearchFormRequest request)
    {
        throw new NotImplementedException();
    }

    public ValueTask<TopHeroesSearchFormRequest?> GetTopHeroesRequestAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask<bool> DeleteCity(string cityId)
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

    public ValueTask SaveCommandCenterProfile(BasicCommandCenterProfile commandCenterProfile)
    {
        throw new NotImplementedException();
    }

    public ValueTask SaveEquipment(IReadOnlyCollection<EquipmentItem> equipment)
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

    public ValueTask<HeroProfileIdentifier?> GetHeroProfileAsync(string heroId)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IReadOnlyCollection<EquipmentItem>> GetEquipmentAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask SaveHeroProfileAsync(HeroProfileIdentifier profile)
    {
        throw new NotImplementedException();
    }

    public ValueTask<UiSettings> GetUiSettingsAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask SaveUiSettingsAsync(UiSettings settings)
    {
        throw new NotImplementedException();
    }

    public ValueTask SaveTempCities(IEnumerable<HohCity> cities)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IReadOnlyCollection<HohCity>> GetTempCities()
    {
        throw new NotImplementedException();
    }

    public ValueTask SaveCityBackup(HohCityBackup cityBackup)
    {
        throw new NotImplementedException();
    }

    public ValueTask SaveCommandCenterProfileBackup(CommandCenterProfileBackup backup)
    {
        throw new NotImplementedException();
    }

    public ValueTask SaveOpenTechnologies(CityId cityId, IReadOnlyCollection<string> openTechnologies)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IReadOnlyCollection<string>> GetOpenTechnologies(CityId cityId)
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

    public ValueTask ScrollTo(ElementReference target, int position, bool smooth = false)
    {
        throw new NotImplementedException();
    }

    public ValueTask SendToGtag(string command, string target, IReadOnlyDictionary<string, object> parameters)
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

internal class DummyCommandCenterProfileSharingService : ICommandCenterProfileSharingService
{
    public Task<ResourceCreatedResponse> ShareAsync(BasicCommandCenterProfile profileDto)
    {
        throw new NotImplementedException();
    }

    public Task<BasicCommandCenterProfile?> GetSharedProfileAsync(string profileId)
    {
        throw new NotImplementedException();
    }
}

internal class DummyLocalStorageBackupService : ILocalStorageBackupService
{
    public ValueTask BackupCities(int currentCityPlannerVersion)
    {
        throw new NotImplementedException();
    }

    public ValueTask BackupCommandCenterProfiles(int currentCommandCenterVersion)
    {
        throw new NotImplementedException();
    }
}
