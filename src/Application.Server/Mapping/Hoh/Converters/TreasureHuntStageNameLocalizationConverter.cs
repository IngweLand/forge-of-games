using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh.Converters;

public class TreasureHuntStageNameLocalizationConverter(IHohGameLocalizationService localizationService)
    : IValueConverter<int, string>
{
    public string Convert(int stageIndex, ResolutionContext context)
    {
        return localizationService.GetTreasureHuntStageName(stageIndex);
    }
}
