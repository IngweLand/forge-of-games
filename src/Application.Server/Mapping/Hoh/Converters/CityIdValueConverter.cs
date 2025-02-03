using AutoMapper;
using Ingweland.Fog.Application.Server.Helpers;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh.Converters;

public class CityIdValueConverter :IValueConverter<string, CityId>
{
    public CityId Convert(string sourceMember, ResolutionContext context)
    {
        return string.IsNullOrWhiteSpace(sourceMember)
            ? CityId.Undefined
            : StringParser.ParseEnumFromString<CityId>(sourceMember, '_');
    }
}
