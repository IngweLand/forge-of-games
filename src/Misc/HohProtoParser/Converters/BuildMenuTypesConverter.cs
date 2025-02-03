using AutoMapper;
using HohProtoParser.Extensions;
using Ingweland.Fog.Models.Hoh.Enums;

namespace HohProtoParser.Converters;

public class BuildMenuTypesConverter : IValueConverter<IEnumerable<string>, IReadOnlyCollection<BuildingType>>
{
    public IReadOnlyCollection<BuildingType> Convert(IEnumerable<string> sourceMember, ResolutionContext context)
    {
        return sourceMember.Select(s => s.ToBuildingType()).ToList();
    }
}
