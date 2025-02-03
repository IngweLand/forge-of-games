using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.City;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IBuildingViewModelFactory
{
    BuildingViewModel Create(BuildingDto source);
}
