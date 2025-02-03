using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.City;

[ProtoContract]
public class LevelUpComponent : ComponentBase
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<int> StarLevels { get; init; } = new List<int>();
}
