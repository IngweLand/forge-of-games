using AutoMapper;
using HohProtoParser.Helpers;
using Ingweland.Fog.Models.Hoh.Enums;

namespace HohProtoParser.Converters;

public class WonderIdValueConverter :IValueConverter<string, WonderId>
{
    public WonderId Convert(string sourceMember, ResolutionContext context)
    {
        return string.IsNullOrWhiteSpace(sourceMember)
            ? WonderId.Undefined
            : StringParser.ParseEnumFromString<WonderId>(sourceMember, '_');
    }
}
