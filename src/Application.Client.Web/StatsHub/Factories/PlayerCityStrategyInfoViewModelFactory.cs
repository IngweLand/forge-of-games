using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Dtos.Hoh.Stats;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Factories;

public class PlayerCityStrategyInfoViewModelFactory(IAssetUrlProvider assetUrlProvider)
    : IPlayerCityStrategyInfoViewModelFactory
{
    public PlayerCityStrategyInfoViewModel Create(PlayerCityStrategyInfoDto dto)
    {
        return new PlayerCityStrategyInfoViewModel
        {
            EventLabel = $"{dto.StartedAt:d} - {dto.EndedAt:d}",
            Id = dto.StrategyId,
            Label = dto.WonderName!,
            IconUrl = assetUrlProvider.GetHohIconUrl(dto.CityId.GetIcon()),
            CityId = dto.CityId,
            Wonder = dto.Wonder,
        };
    }
}
