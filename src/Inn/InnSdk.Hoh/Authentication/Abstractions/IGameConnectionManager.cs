using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

namespace Ingweland.Fog.InnSdk.Hoh.Authentication.Abstractions;

public interface IGameConnectionManager
{
    void AddOrUpdate(GameConnectionSessionData sessionData);

    void Clear();
    GameConnectionSessionData? Get(string worldId);
    void Remove(string worldId);
}
