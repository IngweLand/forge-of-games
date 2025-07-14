using Ingweland.Fog.Application.Core.Factories.Interfaces;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Core.Factories;

public class WonderDtoFactory(IHohGameLocalizationService localizationService) : IWonderDtoFactory
{
    public WonderDto Create(Wonder wonder)
    {
        return new WonderDto()
        {
            Id = wonder.Id,
            CityName = localizationService.GetCityName(wonder.CityId),
            WonderName = localizationService.GetWonderName(wonder.Id.ToString()),
            Levels = wonder.LevelUpComponent.LevelCosts.OrderBy(wlc => wlc.Level)
                .Select(wlc => new WonderLevelDto() {Cost = wlc.Crates}).ToList(),
            Components = wonder.Components.ToList(),
        };
    }
}
