using AutoMapper;
using HohProtoParser.Extensions;
using Ingweland.Fog.Models.Hoh.Enums;

namespace HohProtoParser.Converters;

public class StringToResourceTypeConverter : IValueConverter<string, ResourceType>
{
    public ResourceType Convert(string sourceMember, ResolutionContext context) => sourceMember.ToResourceType();
}
