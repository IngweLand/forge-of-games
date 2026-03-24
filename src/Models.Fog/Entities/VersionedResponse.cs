namespace Ingweland.Fog.Models.Fog.Entities;

public sealed record VersionedResponse<T>(string Version, T? Data);
