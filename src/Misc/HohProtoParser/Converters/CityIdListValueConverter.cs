using AutoMapper;
using Ingweland.Fog.Models.Hoh.Enums;

namespace HohProtoParser.Converters;

public class CityIdListValueConverter : IValueConverter<IEnumerable<string>, HashSet<CityId>>
{
    private readonly CityIdValueConverter _inner = new();

    public HashSet<CityId> Convert(IEnumerable<string> sourceMember, ResolutionContext context)
    {
        return sourceMember?
                   .Select(c => _inner.Convert(c, context))
                   .ToHashSet()
               ?? [];
    }
}
