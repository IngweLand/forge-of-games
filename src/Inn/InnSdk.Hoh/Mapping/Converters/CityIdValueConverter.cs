using AutoMapper;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Helpers;

namespace Ingweland.Fog.InnSdk.Hoh.Mapping.Converters;

public class CityIdValueConverter : IValueConverter<string, CityId>
{
    public CityId Convert(string sourceMember, ResolutionContext context)
    {
        return string.IsNullOrWhiteSpace(sourceMember)
            ? CityId.Undefined
            : HohStringParser.ParseEnumFromString2<CityId>(sourceMember, '_');
    }
}
