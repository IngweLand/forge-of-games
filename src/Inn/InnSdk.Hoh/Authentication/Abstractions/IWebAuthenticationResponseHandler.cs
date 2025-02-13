using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

namespace Ingweland.Fog.InnSdk.Hoh.Authentication.Abstractions;

public interface IWebAuthenticationResponseHandler
{
    GameConnectionSessionData HandleResponse(AccountPlayResponse response, string serverId, string clientVersion);
}
