namespace Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

public class HohServerCredentials
{
    public const string CONFIGURATION_PROPERTY_NAME = "HohServerCredentials";
    public required string Password { get; init; }
    public required string Username { get; init; }
}
