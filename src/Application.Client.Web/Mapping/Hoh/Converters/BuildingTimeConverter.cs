using AutoMapper;
using Ingweland.Fog.Application.Core.Formatters;
using Ingweland.Fog.Application.Core.Formatters.Interfaces;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;

public class BuildingTimeConverter(ITimeFormatters timeFormatters) : IValueConverter<int, string>
{
    public string Convert(int sourceMember, ResolutionContext context)
    {
        return timeFormatters.FromSeconds(sourceMember);
    }
}
