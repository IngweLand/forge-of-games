using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities;

[ProtoContract]
public class LocalizationData
{
    [ProtoMember(1)]
    public required IReadOnlyDictionary<string, IReadOnlyCollection<string>> Entries { get; set; }
}
