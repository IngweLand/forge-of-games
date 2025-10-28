using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Relic;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Dtos.Hoh.Units;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IHeroRelicViewModelFactory
{
    RelicViewModel Create(RelicDto relic, RelicLevelDto level);
}
