using AutoMapper;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Helpers;

namespace HohProtoParser.Converters;

public class WonderIdValueConverter : IValueConverter<string, WonderId>
{
    public WonderId Convert(string sourceMember, ResolutionContext context)
    {
        return string.IsNullOrWhiteSpace(sourceMember)
            ? WonderId.Undefined
            : HohStringParser.ParseEnumFromString2<WonderId>(sourceMember, '_');
    }
}
