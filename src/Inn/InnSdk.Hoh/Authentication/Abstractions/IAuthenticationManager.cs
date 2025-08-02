using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

namespace Ingweland.Fog.InnSdk.Hoh.Authentication.Abstractions;

public interface IAuthenticationManager
{
    Task EnsureAuthenticatedAsync(GameWorldConfig world);
}
