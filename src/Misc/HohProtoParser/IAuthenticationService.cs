namespace Ingweland.Fog.HohProtoParser;

public interface IAuthenticationService
{
    Task<AuthResponse> Authenticate();
}
