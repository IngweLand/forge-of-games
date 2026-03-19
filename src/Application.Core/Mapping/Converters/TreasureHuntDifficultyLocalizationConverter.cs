using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

namespace Ingweland.Fog.Application.Core.Mapping.Converters;

public class TreasureHuntDifficultyLocalizationConverter(IHohGameLocalizationService localizationService)
    : IValueConverter<int, string>
{
    public string Convert(int treasureHuntDifficulty, ResolutionContext context)
    {
        return localizationService.GetTreasureHuntDifficulty(treasureHuntDifficulty);
    }
}
