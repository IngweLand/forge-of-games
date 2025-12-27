using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Dtos.Hoh.PlayerCity;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;

public interface IPlayerCityPropertiesViewModelFactory
{
    PlayerCityPropertiesViewModel Create(PlayerCityPropertiesDto src);
}
