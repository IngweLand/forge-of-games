namespace Ingweland.Fog.Models.Fog.Entities;

public sealed record VersionedResponse<T>(bool NotModified, string CurrentVersion, T? Data);
