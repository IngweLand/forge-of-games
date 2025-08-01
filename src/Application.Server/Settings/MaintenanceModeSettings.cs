namespace Ingweland.Fog.Application.Server.Settings;

public class MaintenanceModeSettings
{
    public const string CONFIGURATION_PROPERTY_NAME = "MaintenanceModeSettings";
    public bool Enabled { get; set; }
    public HashSet<string> AllowedIPs { get; set; } = [];
}
