using System.Collections.Concurrent;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.InnSdk.Hoh.Authentication;

public class GameConnectionManager(ILogger<GameConnectionManager> logger) : IGameConnectionManager
{
    private static readonly TimeSpan SessionTimeout = TimeSpan.FromHours(3);

    private readonly ConcurrentDictionary<string, (GameConnectionSessionData Data, DateTime ExpiresOn)?>
        _configurations = new();

    private readonly SemaphoreSlim _mutex = new(1, 1);

    public void AddOrUpdate(GameConnectionSessionData sessionData)
    {
        logger.LogDebug("Adding or updating session data for ServerId: {ServerId}", sessionData.ServerId);

        var expiresOn = DateTime.UtcNow + SessionTimeout;
        _configurations.AddOrUpdate(sessionData.ServerId, (sessionData, expiresOn), (_, _) => (sessionData, expiresOn));

        logger.LogDebug("Added session data for ServerId: {ServerId} expires on: {ExpiresOn}",
            sessionData.ServerId, expiresOn);
    }

    public void Remove(string worldId)
    {
        logger.LogDebug("Attempting to remove session data for WorldId: {WorldId}", worldId);

        var removed = _configurations.TryRemove(worldId, out _);

        logger.LogDebug("Session data for WorldId: {WorldId} removal result: {Removed}", worldId, removed);
    }

    public void Clear()
    {
        logger.LogDebug("Clearing all session data. Current count: {Count}", _configurations.Count);

        _configurations.Clear();

        logger.LogDebug("All session data cleared");
    }

    public GameConnectionSessionData? Get(string worldId)
    {
        logger.LogDebug("Retrieving session data for WorldId: {WorldId}", worldId);

        _mutex.Wait();
        try
        {
            var data = _configurations.GetValueOrDefault(worldId);
            var now = DateTime.UtcNow;
            if (data != null && now <= data.Value.ExpiresOn)
            {
                logger.LogDebug("Found valid session data for WorldId: {WorldId}, expires on: {ExpiresOn}",
                    worldId, data.Value.ExpiresOn);
                return data.Value.Data;
            }

            if (data != null)
            {
                Remove(worldId);
                logger.LogDebug(
                    "Session data for WorldId: {WorldId} has expired. Expired on: {ExpiresOn}, Current time: {CurrentTime}",
                    worldId, data.Value.ExpiresOn, now);
            }
            else
            {
                logger.LogDebug("No session data found for WorldId: {WorldId}", worldId);
            }

            return null;
        }
        finally
        {
            _mutex.Release();
        }
    }
}
