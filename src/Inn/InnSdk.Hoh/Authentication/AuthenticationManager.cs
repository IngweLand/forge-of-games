using Ingweland.Fog.InnSdk.Hoh.Authentication.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.InnSdk.Hoh.Authentication;

public class AuthenticationManager(
    IServiceScopeFactory scopeFactory,
    IGameConnectionManager connectionManager, ILogger<AuthenticationManager> logger) : IAuthenticationManager
{
    private readonly SemaphoreSlim _authenticationMutex = new(1, 1);

    public async Task EnsureAuthenticatedAsync(GameWorldConfig world)
    {
        await _authenticationMutex.WaitAsync();
        try
        {
            if (connectionManager.Get(world.Id) != null)
            {
                logger.LogDebug("Already authenticated to {WorldId}", world.Id);
                return;
            }

            logger.LogDebug("Authenticating to {WorldId}", world.Id);
            
            using var scope = scopeFactory.CreateScope();
            var authenticationService = scope.ServiceProvider.GetRequiredService<IWebAuthenticationService>();
            await authenticationService.Authenticate(world);
        }
        finally
        {
            _authenticationMutex.Release();
        }
    }
}
