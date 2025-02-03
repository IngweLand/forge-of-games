using AutoMapper;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace HohProtoParser.Converters;

public class UnitValueConverter:IValueConverter<string, Unit>
{
    public Unit Convert(string sourceMember, ResolutionContext context)
    {
        var units = (Dictionary<string, Unit>)context.Items[ContextKeys.UNITS];
        return units[sourceMember];
    }
}
