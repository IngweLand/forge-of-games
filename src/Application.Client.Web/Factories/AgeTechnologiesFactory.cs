using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Research;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.Research;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class AgeTechnologiesFactory(IMapper mapper) : IAgeTechnologiesFactory
{
    public AgeTechnologiesViewModel Create(IEnumerable<TechnologyDto> technologies, AgeDto age)
    {
        return new AgeTechnologiesViewModel()
        {
            AgeName = age.Name,
            AgeColor = age.ToCssColor(),
            Technologies = mapper.Map<IReadOnlyCollection<ResearchCalculatorTechnologyViewModel>>(technologies),
        };
    }
}
