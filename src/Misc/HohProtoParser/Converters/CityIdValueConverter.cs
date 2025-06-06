using AutoMapper;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Helpers;

namespace HohProtoParser.Converters;

public class CityIdValueConverter : IValueConverter<string, CityId>
{
    public CityId Convert(string sourceMember, ResolutionContext context)
    {
        return string.IsNullOrWhiteSpace(sourceMember)
            ? CityId.Undefined
            : HohStringParser.ParseEnumFromString<CityId>(sourceMember, '_');
    }
}
