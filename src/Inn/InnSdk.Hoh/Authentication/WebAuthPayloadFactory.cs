using Ingweland.Fog.InnSdk.Hoh.Authentication.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

namespace Ingweland.Fog.InnSdk.Hoh.Authentication;

public class WebAuthPayloadFactory : IWebAuthPayloadFactory
{
    public LoginRequest CreateForLogin(string username, string password)
    {
        return new LoginRequest(username, password, true);
    }

    public AccountPlayRequest CreateForPlay(string clientVersion)
    {
        return new AccountPlayRequest()
        {
            Meta = new DeviceMeta() {ClientVersion = clientVersion},
        };
    }
}
