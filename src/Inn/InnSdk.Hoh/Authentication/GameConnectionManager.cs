using System.Collections.Concurrent;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

namespace Ingweland.Fog.InnSdk.Hoh.Authentication;

public class GameConnectionManager : IGameConnectionManager
{
    private readonly ConcurrentDictionary<string, GameConnectionSessionData> _configurations = new();

    public void AddOrUpdate(GameConnectionSessionData sessionData)
    {
        _configurations[sessionData.ServerId] = sessionData;
    }

    public void Remove(string serverId)
    {
        _configurations.TryRemove(serverId, out _);
    }

    public void Clear()
    {
        _configurations.Clear();
    }

    public GameConnectionSessionData? Get(string serverId)
    {
        return _configurations.GetValueOrDefault(serverId);
    }
}
