namespace Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

public record AccountPlayRequest
{
    public bool CreateDeviceToken { get; init; }
    public DeviceMeta Meta { get; init; } = new();
    public string Network { get; init; } = "BROWSER_SESSION";
    public string Token { get; init; } = string.Empty;
    public string? WorldId { get; init; }
}
