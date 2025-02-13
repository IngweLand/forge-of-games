using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;

namespace Ingweland.Fog.InnSdk.Hoh.Net.Abstractions;

public interface IGameApiClient
{
    Task<string> SendForJsonAsync(GameWorldConfig world, string url, byte[] payload);
    Task<byte[]> SendForProtobufAsync(GameWorldConfig world, string url, byte[] payload);
}
