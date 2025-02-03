using Ingweland.Fog.Models.Hoh.Entities.City;
using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Abstractions;

[ProtoContract]
[ProtoInclude(100, typeof(BuildingUnitProviderComponent))]
[ProtoInclude(101, typeof(ConstructionComponent))]
[ProtoInclude(102, typeof(CultureComponent))]
[ProtoInclude(103, typeof(GrantWorkerComponent))]
[ProtoInclude(104, typeof(HeroAbilityTrainingComponent))]
[ProtoInclude(105, typeof(HeroBuildingBoostComponent))]
[ProtoInclude(106, typeof(ProductionComponent))]
[ProtoInclude(107, typeof(UpgradeComponent))]
[ProtoInclude(108, typeof(BoostResourceComponent))]
[ProtoInclude(109, typeof(CultureBoostComponent))]
[ProtoInclude(110, typeof(LevelUpComponent))]
public abstract class ComponentBase
{
}
