using AutoMapper;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Shared.Helpers;

namespace HohProtoParser.Converters;

public class AgeValueConverter:IValueConverter<string, Age>
{
    public Age Convert(string sourceMember, ResolutionContext context)
    {
        var ages = (Dictionary<string, Age>)context.Items[ContextKeys.AGES];
        return ages[HohStringParser.GetConcreteId(sourceMember)];
    }
}
