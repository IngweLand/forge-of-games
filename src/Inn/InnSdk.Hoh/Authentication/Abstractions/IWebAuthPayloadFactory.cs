using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

namespace Ingweland.Fog.InnSdk.Hoh.Authentication.Abstractions;

public interface IWebAuthPayloadFactory
{
    LoginRequest CreateForLogin(string username, string password);
    AccountPlayRequest CreateForPlay(string clientVersion);
}
