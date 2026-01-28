using Ingweland.Fog.Application.Client.Web.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Dtos.Hoh;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface ICommunityCityStrategyViewModelFactory
{
    CommunityCityStrategyViewModel Create(CommunityCityStrategyDto dto,
        IReadOnlyDictionary<string, AgeViewModel> ages);
}
