namespace Ingweland.Fog.Application.Server.Settings;

public class StorageSettings
{
    public const string CONFIGURATION_PROPERTY_NAME = "StorageSettings";
    public required string CityPlannerCitiesTable { get; set; }
    public required string CommandCenterProfilesTable { get; set; }
    public required string ConnectionString { get; set; }
    public required string HohStartupDataTable { get; set; }
    public required string PlayerRankingsTable { get; set; }
    public required string AllianceRankingsTable { get; set; }
    public required string AllianceRankingsRawDataTable { get; set; }
}
