using ProtoBuf;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;

namespace Ingweland.Fog.Models.Hoh.Entities.Research;

[ProtoContract]
public class ResearchComponent
{
    [ProtoMember(1)]
    public required IReadOnlyCollection<ResourceAmount> Costs { get; init; }

    [ProtoMember(2)]
    public required IReadOnlyCollection<string> ParentTechnologies { get; init; }

    [ProtoMember(3)]
    public required IReadOnlyCollection<ResearchRewardBase> Rewards { get; init; }
}
