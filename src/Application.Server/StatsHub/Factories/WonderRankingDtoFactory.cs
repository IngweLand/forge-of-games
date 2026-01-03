using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.StatsHub.Factories;

public class WonderRankingDtoFactory(IHohGameLocalizationService localizationService) : IWonderRankingDtoFactory
{
    public WonderRankingDto Create(EventCityWonderRanking entity, InGameEventEntity inGameEvent)
    {
        var wonderId = Enum.GetValues<WonderId>().First(x => inGameEvent.InGameDefinitionId.EndsWith(x.ToString()));
        return new WonderRankingDto()
        {
            Level = entity.WonderLevel,
            Wonder = wonderId,
            WonderName = localizationService.GetWonderName(wonderId.ToString()),
            StartedAt = inGameEvent.StartAt,
            EndedAt = inGameEvent.EndAt,
        };
    }
}
