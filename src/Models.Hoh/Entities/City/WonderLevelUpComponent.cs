using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class WonderLevelUpComponent
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<WonderLevelCost> LevelCosts { get; init; }
}
