using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Units;

[ProtoContract]
public class RelicHeroEquipFilter
{
    [ProtoMember(1)]
    public required HeroClassId HeroClassId { get; init; }
}
