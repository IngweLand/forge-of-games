using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

namespace Ingweland.Fog.Application.Core.Mapping.Converters;

public class TechnologyNameLocalizationConverter(IHohGameLocalizationService localizationService)
    : IValueConverter<string, string>
{
    public string Convert(string technologyId, ResolutionContext context)
    {
        var name = localizationService.GetTechnologyName(technologyId);
        return name;
    }
}
