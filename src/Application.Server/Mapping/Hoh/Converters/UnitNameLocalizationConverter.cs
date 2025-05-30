using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh.Converters;

public class UnitNameLocalizationConverter(IHohGameLocalizationService localizationService)
    : IValueConverter<string, string>
{
    public string Convert(string unitId, ResolutionContext context)
    {
        return localizationService.GetUnitName(unitId);
    }
}
