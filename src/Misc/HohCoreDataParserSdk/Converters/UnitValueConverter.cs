using AutoMapper;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace Ingweland.Fog.HohCoreDataParserSdk.Converters;

public class UnitValueConverter:IValueConverter<string, Unit>
{
    public Unit Convert(string sourceMember, ResolutionContext context)
    {
        var units = (Dictionary<string, Unit>)context.Items[ContextKeys.UNITS];
        return units[sourceMember];
    }
}
