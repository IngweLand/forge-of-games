namespace Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

public record AccountPlayResponse
{
    public int AccountId { get; init; }
    public string GameId { get; init; }
    public string SessionId { get; init; }
    public string Url { get; init; }
    public int WorldId { get; init; }
}
