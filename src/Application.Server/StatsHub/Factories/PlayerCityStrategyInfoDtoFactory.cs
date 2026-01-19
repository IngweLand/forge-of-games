using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.StatsHub.Factories;

public class PlayerCityStrategyInfoDtoFactory(IHohGameLocalizationService localizationService)
    : IPlayerCityStrategyInfoDtoFactory
{
    public PlayerCityStrategyInfoDto Create(EventCityStrategy strategy)
    {
        var wonderId = Enum.GetValues<WonderId>()
            .First(x => strategy.InGameEvent.InGameDefinitionId.EndsWith(x.ToString()));
        return new PlayerCityStrategyInfoDto
        {
            StrategyId = strategy.Id,
            Wonder = wonderId,
            WonderName = localizationService.GetWonderName(wonderId.ToString()),
            StartedAt = strategy.InGameEvent.StartAt,
            EndedAt = strategy.InGameEvent.EndAt,
            CityId = strategy.CityId,
        };
    }
}
