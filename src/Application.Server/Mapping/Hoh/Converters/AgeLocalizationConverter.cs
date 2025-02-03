using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh.Converters;

public class AgeLocalizationConverter(IHohGameLocalizationService localizationService)
    : IValueConverter<string?, string>
{
    public string Convert(string? ageId, ResolutionContext context)
    {
        return ageId == null ? string.Empty : localizationService.GetAgeName(ageId);
    }
}
