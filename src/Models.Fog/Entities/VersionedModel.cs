using ProtoBuf;

namespace Ingweland.Fog.Models.Fog.Entities;

[ProtoContract]
public abstract class VersionedModel
{
    [ProtoMember(1)]
    public int SchemaVersion { get; set; } = 0;
}
