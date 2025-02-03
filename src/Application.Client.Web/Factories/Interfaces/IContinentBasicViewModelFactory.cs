using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Battle;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IContinentBasicViewModelFactory
{
    IReadOnlyCollection<ContinentBasicViewModel> CreateContinents(IEnumerable<ContinentBasicDto> continents);
}
