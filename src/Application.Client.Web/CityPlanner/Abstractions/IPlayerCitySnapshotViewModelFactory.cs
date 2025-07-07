using Ingweland.Fog.Application.Client.Web.CityPlanner.Inspirations;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Dtos.Hoh.PlayerCity;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IPlayerCitySnapshotViewModelFactory
{
    PlayerCitySnapshotBasicViewModel Create(PlayerCitySnapshotBasicDto snapshot, AgeViewModel age);
}
