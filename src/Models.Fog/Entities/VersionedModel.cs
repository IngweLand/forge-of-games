namespace Ingweland.Fog.Models.Fog.Entities;

public abstract class VersionedModel
{
    public int SchemaVersion { get; set; } = 0;
}

