namespace Ingweland.Fog.Application.Client.Web.Services.Abstractions;

public interface ILocalStorageBackupService
{
    ValueTask BackupCities(int currentCityPlannerVersion);
    ValueTask BackupCommandCenterProfiles(int currentCommandCenterVersion);
}