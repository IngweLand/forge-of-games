namespace Ingweland.Fog.Application.Server.Caching;

public sealed record VersionedCachedResource<T>(string Version, T Data);
