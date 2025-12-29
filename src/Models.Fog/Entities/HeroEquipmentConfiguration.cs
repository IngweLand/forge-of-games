using System.Linq;
using ProtoBuf;

namespace Ingweland.Fog.Models.Fog.Entities;

[ProtoContract]
public class HeroEquipmentConfiguration
{
    [ProtoMember(1)]
    public int? GarmentId { get; set; }

    [ProtoMember(2)]
    public int? HandId { get; set; }

    [ProtoMember(3)]
    public int? HatId { get; set; }

    [ProtoMember(4)]
    public required string HeroId { get; init; }

    [ProtoMember(5)]
    public required string Id { get; init; }

    [ProtoMember(6)]
    public bool IsInGame { get; init; }

    [ProtoMember(7)]
    public int? NeckId { get; set; }

    [ProtoMember(8)]
    public int? RingId { get; set; }
    
    public IReadOnlyCollection<int> GetIds()
    {
        return new[] { GarmentId, HandId, HatId, NeckId, RingId }
            .Where(id => id.HasValue)
            .Select(id => id!.Value)
            .ToArray();
    }
}
