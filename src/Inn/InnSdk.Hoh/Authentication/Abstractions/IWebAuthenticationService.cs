using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

namespace Ingweland.Fog.InnSdk.Hoh.Authentication.Abstractions;

public interface IWebAuthenticationService
{
    Task Authenticate(GameWorldConfig world);
}
