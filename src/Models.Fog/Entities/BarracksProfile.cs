using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Models.Fog.Entities;

[ProtoContract]
public class BarracksProfile
{
    [ProtoMember(1)]
    public IDictionary<BuildingGroup, int> Levels { get; init; } = new Dictionary<BuildingGroup, int>
    {
        {BuildingGroup.RangedBarracks, 1},
        {BuildingGroup.SiegeBarracks, 2},
        {BuildingGroup.CavalryBarracks, 1},
        {BuildingGroup.HeavyInfantryBarracks, 2},
        {BuildingGroup.InfantryBarracks, 1},
    };
}
