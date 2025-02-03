namespace Ingweland.Fog.Application.Server.Settings;

public class ResourceSettings
{
    public const string CONFIGURATION_PROPERTY_NAME = "ResourceSettings";

    public required string BaseUrl { get; set; }
    public required string HohDataPath { get; set; }
    public required string HohLocalizationsDirectory { get; set; }
}
