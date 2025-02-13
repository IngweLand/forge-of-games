using Ingweland.Fog.InnSdk.Hoh.Authentication.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

namespace Ingweland.Fog.InnSdk.Hoh.Authentication;

public class WebAuthenticationResponseHandler : IWebAuthenticationResponseHandler
{
    public GameConnectionSessionData HandleResponse(AccountPlayResponse response, string serverId, string clientVersion)
    {
        return new GameConnectionSessionData(ServerId: serverId, ClientVersion: clientVersion,
            SessionId: response.SessionId);
    }
}
