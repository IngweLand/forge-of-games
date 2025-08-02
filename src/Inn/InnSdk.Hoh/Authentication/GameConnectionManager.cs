using System.Collections.Concurrent;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

namespace Ingweland.Fog.InnSdk.Hoh.Authentication;

public class GameConnectionManager : IGameConnectionManager
{
    private readonly ConcurrentDictionary<string, GameConnectionSessionData> _configurations = new();

    public void AddOrUpdate(GameConnectionSessionData sessionData)
    {
        _configurations.AddOrUpdate(sessionData.ServerId, sessionData, (_, _) => sessionData);
    }

    public void Remove(string worldId)
    {
        _configurations.TryRemove(worldId, out _);
    }

    public void Clear()
    {
        _configurations.Clear();
    }

    public GameConnectionSessionData? Get(string worldId)
    {
        return _configurations.GetValueOrDefault(worldId);
    }
}
