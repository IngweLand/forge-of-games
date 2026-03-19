using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

namespace Ingweland.Fog.Application.Core.Mapping.Converters;

public class AgeLocalizationConverter(IHohGameLocalizationService localizationService)
    : IValueConverter<string?, string>
{
    public string Convert(string? ageId, ResolutionContext context)
    {
        return ageId == null ? string.Empty : localizationService.GetAgeName(ageId);
    }
}
