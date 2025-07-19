using Blazored.LocalStorage;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;

namespace Ingweland.Fog.WebApp.Client.Services;

public class LocalStorageBackupService(IPersistenceService persistenceService, ILocalStorageService localStorageService)
    : ILocalStorageBackupService
{
    private const string LastCityPlannerVersionBackupKey = "LastCityPlannerVersionBackup";
    private const string LastCommandCenterVersionBackupKey = "LastCommandCenterVersionBackup";

    public async ValueTask BackupCities(int currentCityPlannerVersion)
    {
        var lastVersion = await localStorageService.GetItemAsync<int>(LastCityPlannerVersionBackupKey);
        if (currentCityPlannerVersion <= lastVersion)
        {
            return;
        }

        var cityIds = (await persistenceService.GetCities()).Select(src => src.Id);
        foreach (var cityId in cityIds)
        {
            var city = await persistenceService.LoadCity(cityId);
            var backup = new HohCityBackup
            {
                City = city!,
                CityPlannerVersion = currentCityPlannerVersion,
            };
            await persistenceService.SaveCityBackup(backup);
        }

        await localStorageService.SetItemAsync(LastCityPlannerVersionBackupKey, currentCityPlannerVersion);
    }

    public async ValueTask BackupCommandCenterProfiles(int currentCommandCenterVersion)
    {
        var lastVersion = await localStorageService.GetItemAsync<int>(LastCommandCenterVersionBackupKey);
        if (currentCommandCenterVersion <= lastVersion)
        {
            return;
        }

        var profiles = await persistenceService.GetProfilesAsync();
        foreach (var profile in profiles)
        {
            var backup = new CommandCenterProfileBackup
            {
                Profile = profile,
                CommandCenterVersion = currentCommandCenterVersion,
            };
            await persistenceService.SaveCommandCenterProfileBackup(backup);
        }

        await localStorageService.SetItemAsync(LastCommandCenterVersionBackupKey, currentCommandCenterVersion);
    }
}
