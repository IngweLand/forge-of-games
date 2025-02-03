using AutoMapper;
using HohProtoParser.Helpers;
using Ingweland.Fog.Models.Hoh.Enums;

namespace HohProtoParser.Converters;

public class CityIdValueConverter :IValueConverter<string, CityId>
{
    public CityId Convert(string sourceMember, ResolutionContext context)
    {
        return string.IsNullOrWhiteSpace(sourceMember)
            ? CityId.Undefined
            : StringParser.ParseEnumFromString<CityId>(sourceMember, '_');
    }
}
