using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;
using ProtoBuf;

namespace Ingweland.Fog.Dtos.Hoh.City;

[ProtoContract]
public class BuildingDto
{
    private CultureComponent? _cultureComponent;

    private bool _cultureComponentSearched;

    [ProtoMember(1)]
    public required AgeDto? Age { get; init; }

    [ProtoMember(15)]
    public BuildingBuffDetails? BuffDetails { get; init; }

    [ProtoMember(3)]
    public required HashSet<CityId> CityIds { get; init; }

    [ProtoMember(4)]
    public required IReadOnlyCollection<ComponentBase> Components { get; init; }

    [ProtoIgnore]
    public CultureComponent? CultureComponent
    {
        get
        {
            if (!_cultureComponentSearched)
            {
                _cultureComponent = Components.OfType<CultureComponent>().FirstOrDefault();
                _cultureComponentSearched = true;
            }

            return _cultureComponent;
        }
    }

    [ProtoMember(14)]
    public required ExpansionSubType ExpansionSubType { get; init; }

    [ProtoMember(10)]
    public required BuildingGroup Group { get; init; }

    [ProtoMember(11)]
    public required string GroupName { get; init; }

    [ProtoMember(5)]
    public required string Id { get; init; }

    [ProtoMember(6)]
    public int Length { get; init; }

    [ProtoMember(7)]
    public int Level { get; init; }

    [ProtoMember(8)]
    public required string Name { get; init; }

    [ProtoMember(12)]
    public required BuildingType Type { get; init; }

    [ProtoMember(13)]
    public required string TypeName { get; init; }

    [ProtoMember(9)]
    public int Width { get; init; }
}
