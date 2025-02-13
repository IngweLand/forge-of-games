namespace Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

public record DeviceMeta
{
    public string ClientVersion { get; init; } = string.Empty;
    public string Device { get; init; } = "browser";
    public string DeviceHardware { get; init; } = "browser";
    public string DeviceManufacturer { get; init; } = "none";
    public string DeviceName { get; init; } = "browser";
    public string Locale { get; init; } = "en_DK";
    public string NetworkType { get; init; } = "wlan";
    public string OperatingSystemName { get; init; } = "browser";
    public string OperatingSystemVersion { get; init; } = "1";
    public string? Platform { get; init; }
    public string UserAgent { get; init; } = "forgeofgames";
}
