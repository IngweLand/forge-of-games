using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Core.CityPlanner.Abstractions;

public interface ICityMapStateCoreFactory
{
    CityMapStateCore Create(IReadOnlyCollection<BuildingDto> buildings,
        IReadOnlyCollection<AgeDto> ages,
        HohCity city,
        WonderDto? wonder);
}
