using AutoMapper;
using Ingweland.Fog.HohCoreDataParserSdk.Extensions;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.HohCoreDataParserSdk.Converters;

public class StringToResourceTypeConverter : IValueConverter<string, ResourceType>
{
    public ResourceType Convert(string sourceMember, ResolutionContext context) => sourceMember.ToResourceType();
}
