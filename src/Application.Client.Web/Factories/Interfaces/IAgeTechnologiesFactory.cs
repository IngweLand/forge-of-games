using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Research;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.Research;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IAgeTechnologiesFactory
{
    AgeTechnologiesViewModel Create(IEnumerable<TechnologyDto> technologies, AgeDto age);
}
