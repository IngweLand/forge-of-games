namespace Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

public record GameWorldConfig(string Server, int WorldNumber, string SignInSubdomain)
{
    public string Id => $"{Server}{WorldNumber}";
}
