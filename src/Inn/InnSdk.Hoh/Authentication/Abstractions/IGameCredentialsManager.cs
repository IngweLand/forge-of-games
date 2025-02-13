using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

namespace Ingweland.Fog.InnSdk.Hoh.Authentication.Abstractions;

public interface IGameCredentialsManager
{
    Task<HohServerCredentials?> GetAsync(GameWorldConfig world);
    Task ReportAuthIssueAsync(HohServerCredentials credentials, AuthErrorCode errorCode);
}
