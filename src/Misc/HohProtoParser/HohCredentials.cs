namespace Ingweland.Fog.HohProtoParser;

public class HohCredentials
{
    public const string CONFIGURATION_PROPERTY_NAME = "HohCredentials";
    public required string Username { get; init; }
    public required string Password { get; init; }
}
