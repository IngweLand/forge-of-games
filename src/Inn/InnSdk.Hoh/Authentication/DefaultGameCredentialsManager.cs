using Ingweland.Fog.InnSdk.Hoh.Authentication.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Microsoft.Extensions.Options;

namespace Ingweland.Fog.InnSdk.Hoh.Authentication;

public class DefaultGameCredentialsManager(IOptions<List<HohServerCredentials>> credentialsOptions)
    : IGameCredentialsManager
{
    public Task<HohServerCredentials?> GetAsync(GameWorldConfig world)
    {
        return Task.FromResult(credentialsOptions.Value.FirstOrDefault(src => src.ServerId == world.Id));
    }

    public Task ReportAuthIssueAsync(HohServerCredentials credentials, AuthErrorCode errorCode)
    {
        throw new NotImplementedException();
    }
}
