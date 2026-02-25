namespace Ingweland.Fog.Application.Server.Settings;

public class PatreonSettings
{
    public const string CONFIGURATION_PROPERTY_NAME = "PatreonSettings";
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
}
