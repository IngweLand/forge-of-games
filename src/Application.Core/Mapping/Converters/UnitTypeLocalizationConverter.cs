using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Core.Mapping.Converters;

public class UnitTypeLocalizationConverter : IValueConverter<UnitType, string>
{
    private readonly IHohGameLocalizationService _localizationService;

    public UnitTypeLocalizationConverter(IHohGameLocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    public string Convert(UnitType unitType, ResolutionContext context) => _localizationService.GetUnitType(unitType);
}
